using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using PreguntameAPIv3.LogicaAccesoDatos.Models;
using PreguntameAPIv3.LogicaAccesoDatos.Repositorios;
using PreguntameAPIv3.LogicaAplicacion.CasosDeUso;
using PreguntameAPIv3.LogicaAplicacion.ICasosDeUso;
using PreguntameAPIv3.LogicaNegocio.IRepositorios;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// DBContext
builder.Services.AddDbContext<PreguntameDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("PreguntameDB"))
);

// Repositorios
builder.Services.AddScoped<IUsuariosRepositorio<Usuario>, UsuariosRepositorio>();
builder.Services.AddScoped<IInteraccionesRepositorio, InteraccionesRepositorio>();

// Casos de uso (Services)
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IInteraccionesService, InteraccionesService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfig:Secret"])),
            ClockSkew = TimeSpan.Zero
        };

        opts.Events = new JwtBearerEvents
        {
            // Obtener el token de la cookie
            OnMessageReceived = (context) =>
            {
                context.Token = context.Request.Cookies["JWTtoken"];
                return Task.CompletedTask;
            },
            // Personalizar respuesta para cuando falla [Authorize] en controladores
            OnChallenge = async (context) =>
            {
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                Dictionary<string, string> errorData = new Dictionary<string, string>
                {
                    { "Message", "Fallo en la autenticación del token" }
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorData));
            }
        };
    });

builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policyBuilder => policyBuilder.WithOrigins("http://localhost:5173") // URL Front
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials()); // Habilita el uso de credenciales (cookies)
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Servir fotos de usuarios
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, @"..\..\..\LogicaAccesoDatos\FotosUsuarios")),
    RequestPath = "/api/fotosusuarios"
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar CORS
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
