using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.Compartido.DTOs.Usuario;
using PreguntameAPIv3.LogicaAplicacion.ICasosDeUso;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.StaticFiles;
using PreguntameAPIv3.LogicaNegocio.Validadores;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using System.Text;

namespace PreguntameAPIv3.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUsuariosService _usuariosService;
        private IInteraccionesService _interaccionesService;
        public UsuariosController(IUsuariosService usuariosService, IInteraccionesService interaccionesService)
        {
            _usuariosService = usuariosService;
            _interaccionesService = interaccionesService;
        }

        [HttpPost("registrar-usuario")]
        public async Task<IActionResult> CreateUsuario([FromBody] UsuarioInsertDTO usuarioInsertDTO)
        {
            try
            {
                await _usuariosService.RegistrarUsuario(usuarioInsertDTO);
                return Ok(new { Message = "Usuario registrado con éxito" });
            }
            catch (UsuarioException uex)
            {
                return BadRequest(new { uex.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("iniciar-sesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] IniciarSesionDTO iniciarSesionDTO)
        {
            try
            {
                var respuesta = await _usuariosService.IniciarSesion(iniciarSesionDTO);
                var token = respuesta.Item1;
                var datosUsuario = respuesta.Item2;

                Response.Cookies.Append("jwttoken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                return Ok(datosUsuario);
            }
            catch (UsuarioException uex)
            {
                return BadRequest(new { uex.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize]
        [HttpGet("notificar-preguntas")]
        public async Task<IActionResult> GetNPreguntasPendientes()
        {
            var username = User.FindFirst("username")?.Value;
            if (username == null)
            {
                return Unauthorized(new { Message = "No se reconoce al usuario que solicita esta acción" });
            }

            try
            {
                var nPreguntas = await _interaccionesService.GetPreguntasUsername(username);
                var contador = nPreguntas.ToList();
                return Ok(new { NPreguntas = contador.Count });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("cerrar-sesion")]
        public IActionResult CerrarSesion()
        {
            Response.Cookies.Append("jwtToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(-1)
            });
            return Ok(new { Message = "Sesión cerrada" });
        }

        [Authorize]
        [HttpPatch("editar-perfil")]
        public async Task<IActionResult> EditarDatosUsuario([FromBody] EditarUsuarioDTO editarUsuarioDTO)
        {
            var username = User.FindFirst("username")?.Value;
            if(username  == null)
            {
                return Unauthorized(new { Message = "No se reconoce al usuario que solicita esta acción" });
            }

            try
            {
                var usuarioEditado = await _usuariosService.EditarDatosUsuario(username, editarUsuarioDTO);

                await Console.Out.WriteLineAsync("NUEVO NOMBRE");
                await Console.Out.WriteLineAsync(usuarioEditado.Nombre);
                return Ok(usuarioEditado);
            }
            catch(UsuarioException uex)
            {
                return BadRequest(new { Message =  uex.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message =  ex.Message });
            }
        }

        [Authorize]
        [HttpPost("cambiar-foto-usuario")]
        public async Task<IActionResult> CambiarFotoUsuario([FromForm] IFormFile fotoUsuario)
        {
            var username = User.FindFirst("username")?.Value;
            if (username == null)
            {
                return Unauthorized(new { Message = "No se reconoce al usuario que solicita esta acción" });
            }

            if (fotoUsuario == null || fotoUsuario.Length == 0)
            {
                return BadRequest(new { Message = "No se seleccionó ninguna imágen" });
            }

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extensionArchivo = Path.GetExtension(fotoUsuario.FileName);
            if (string.IsNullOrEmpty(extensionArchivo) || !permittedExtensions.Contains(extensionArchivo.ToLowerInvariant()))
            {
                return BadRequest(new { Message = "Tipo de archivo no permitido" });
            }

            try
            {
                // Ruta base del directorio donde está el ejecutable
                var carpetaBase = AppContext.BaseDirectory;

                // Ruta relativa comenzando desde el proyecto
                var carpetaProyecto = Path.GetFullPath(Path.Combine(carpetaBase, @"..\..\..\"));

                // Ruta a carpeta donde se guardan las fotos
                var carpetaFotos = Path.Combine(carpetaProyecto, "LogicaAccesoDatos/FotosUsuarios");

                // Asignar a la foto el nombre igual al username
                string nombreFoto = $"{username}{extensionArchivo}"; // var extensionArchivo = Path.GetExtension(fotoUsuario.FileName);


                // Primero buscar imagen por username sin importar extension y si existe, eliminara
                foreach (var archivo in Directory.GetFiles(carpetaFotos, $"{username}.*"))
                {
                    System.IO.File.Delete(archivo);
                }

                // Ahora asignar la foto con su nombre y extensión a la carpeta y guardarla
                var rutaArchivo = Path.Combine(carpetaFotos, nombreFoto);


                // Redimensionar la imagen antes de guardarla
                using (var image = Image.Load(fotoUsuario.OpenReadStream())) // Cargar la imagen en memoria
                {
                    var ratio = (15000 / image.Width) / 100; 
                    // Redimensionar la imagen manteniendo la relación de aspecto (50% en este ejemplo)
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(150, image.Height * ratio), // Aquí ajustas el tamaño según tus necesidades
                        Mode = ResizeMode.Max // Mantener la relación de aspecto
                    }));


                    // Primero seleccionar el encoder correcto según extensión de la imágen
                    // Y Guardar la imagen redimensionada
                    switch (extensionArchivo.ToLowerInvariant())
                    {
                        case ".png":
                            await image.SaveAsync(rutaArchivo, new PngEncoder());
                            break;
                        case ".jpeg":
                        case ".jpg":
                            await image.SaveAsync(rutaArchivo, new JpegEncoder());
                            break;
                        case ".webp":
                            await image.SaveAsync(rutaArchivo, new WebpEncoder());
                            break;
                        default:
                            return BadRequest(new { Message = "Tipo de archivo no soportado" });
                    }
                }

                //using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                //{
                //    await fotoUsuario.CopyToAsync(stream);
                //}

                return Ok(new { Message = "Foto cambiada con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("foto-usuario/{username}")]
        public IActionResult ObtenerFotoUsuario(string username)
        {
            // Ruta base del directorio donde están las imágenes
            var carpetaBase = AppContext.BaseDirectory;
            var carpetaFotos = Path.GetFullPath(Path.Combine(carpetaBase, @"..\..\..\LogicaAccesoDatos\FotosUsuarios"));

            // Buscar archivo con el nombre del usuario
            var archivos = Directory.GetFiles(carpetaFotos, $"{username}.*");
            if (archivos.Length == 0)
            {
                return NotFound(new { Message = "No se encontró una foto para este usuario"});
            }

            // Devuelve el primer archivo encontrado
            var archivo = archivos[0];
            var mimeType = "image/png"; // Puedes mejorar esto para detectar el MIME type adecuado
            return File(System.IO.File.ReadAllBytes(archivo), mimeType);
        }




        [Authorize]
        [HttpGet("preguntas")]
        public async Task<IActionResult> GetPreguntasUsuario()
        {
            var username = User.FindFirst("username")?.Value;
            if (username == null)
            {
                return Unauthorized(new { Message = "No se reconoce al usuario que solicita esta acción" });
            }

            try
            {
                var liPreguntas = await _interaccionesService.GetPreguntasUsername(username);
                return Ok(liPreguntas);
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("pregunta/{preguntaid}")]
        public async Task<IActionResult> GetPreguntaPorIdUsuario(string preguntaid)
        {
            var username = User.FindFirst("username")?.Value;
            if (username == null)
            {
                return Unauthorized(new { Message = "No se reconoce al usuario que solicita esta acción" });
            }

            try
            {
                var encontrarPregunta = await _interaccionesService.GetPreguntaPorIdyUsername(username, preguntaid);
                return Ok(encontrarPregunta);
            }
            catch(PreguntaException pex)
            {
                return NotFound(new { Message = pex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); 
            }
        }

        [Authorize]
        [HttpPost("responder-pregunta")]
        public async Task<IActionResult> ResponderPregunta([FromBody] RespuestaInsertDTO respuestaInsertDTO)
        {
            var username = User.FindFirst("username")?.Value;
            if (username == null)
            {
                return Unauthorized(new { Message = "No se reconoce al usuario que solicita esta acción" });
            }

            try
            {
                await _interaccionesService.InsertRespuesta(username, respuestaInsertDTO);
                return Ok(new { Message = "Respuesta respondida con éxito" });
            }
            catch(RespuestaException rex)
            {
                return BadRequest(new { Message = rex.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("perfil/{username}")]
        public async Task<IActionResult> GetPerfilUsername(string username)
        {
            var token = HttpContext.Request.Cookies["jwttoken"];
            Guid? usuarioLogId = null;
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                var usuarioidEnToken = jwtToken.Claims.FirstOrDefault(a => a.Type == "usuarioid")?.Value;
                if (usuarioidEnToken != null)
                {
                    usuarioLogId = new Guid(usuarioidEnToken);
                }
            }

            try
            {
                var perfilUsuario = await _usuariosService.GetPerfilUsuario(username, usuarioLogId);
                if(perfilUsuario == null)
                {
                    return NotFound(new { Message = "Usuario no encontrado" });
                }
                return Ok(perfilUsuario);
            }
            catch(UsuarioException uex)
            {
                if(uex.Message == "Usuario no encontrado")
                {
                    return NotFound(new { Message = "Usuario no encontrado" });
                }
                return BadRequest(new { Message = uex.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("buscar-usuarios/{busqueda}")]
        public async Task<IActionResult> BuscarUsuarios(string busqueda)
        {
            if (RegexGenerales.ValidarCaracteresProhibidos(busqueda))
            {
                return BadRequest(new { Message = "Los caracteres (' \" \\ ; | < > -) están prohibidos" });
            }
            if (String.IsNullOrEmpty(busqueda))
            {
                return BadRequest(new { Message = "El texto de búsqueda se encuentra vacío" });
            }

            try
            {
                var IEUsuarios = await _usuariosService.BuscarUsuarios(busqueda);
                return Ok(IEUsuarios);
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message =  ex.Message });
            }
        }

    }
}
