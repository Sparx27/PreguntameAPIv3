using PreguntameAPIv3.Compartido.DTOs.Usuario;
using PreguntameAPIv3.LogicaAccesoDatos.Models;
using PreguntameAPIv3.LogicaAplicacion.ICasosDeUso;
using PreguntameAPIv3.LogicaNegocio.IRepositorios;
using PreguntameAPIv3.LogicaNegocio.Validadores.Usuario;
using PreguntameAPIv3.Compartido.Mappers;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace PreguntameAPIv3.LogicaAplicacion.CasosDeUso
{
    public class UsuariosService : IUsuariosService
    {
        private IUsuariosRepositorio<Usuario> _usuariosRepositorio;
        private IInteraccionesService _interaccionesService;
        private IConfiguration _config;
        public UsuariosService(IUsuariosRepositorio<Usuario> usuariosRepositorio, IConfiguration config, IInteraccionesService interaccionesService)
        {
            _usuariosRepositorio = usuariosRepositorio;
            _interaccionesService = interaccionesService;
            _config = config;
        }

        public async Task RegistrarUsuario(UsuarioInsertDTO usuarioInsertDTO)
        {
            usuarioInsertDTO.Username = usuarioInsertDTO.Username?.Trim();
            usuarioInsertDTO.Email = usuarioInsertDTO.Email?.Trim();
            usuarioInsertDTO.UPassword = usuarioInsertDTO.UPassword?.Trim();
            usuarioInsertDTO.Apellido = usuarioInsertDTO.Apellido?.Trim();
            usuarioInsertDTO.Nombre = usuarioInsertDTO.Nombre?.Trim();

            ValidarUsuarioInsert.Validar(usuarioInsertDTO);
            // Hashear password
            usuarioInsertDTO.UPassword = BCrypt.Net.BCrypt.HashPassword(usuarioInsertDTO.UPassword, workFactor: 10);

            try
            {
                await _usuariosRepositorio.InsertUsuario(UsuarioMapper.InsertToUsuario(usuarioInsertDTO));
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                if(sqlEx.Message.Contains("UQ_Email"))
                {
                    throw new UsuarioException("El Email ya fue registrado antes");
                }
                if (sqlEx.Message.Contains("UQ_Username"))
                {
                    throw new UsuarioException("El Username ya fue registrado antes");
                }
                throw ex;
            }
        }

        public async Task<(string, DatosUsuarioDTO)> IniciarSesion(IniciarSesionDTO iniciarSesionDTO)
        {
            iniciarSesionDTO.Username = iniciarSesionDTO.Username?.Trim();
            iniciarSesionDTO.UPassword = iniciarSesionDTO.UPassword?.Trim();

            ValidarIniciarSesion.Validar(iniciarSesionDTO);
            var encontrarUsuario = await _usuariosRepositorio.GetUsuarioUsername(iniciarSesionDTO.Username);

            if(encontrarUsuario == null)
            {
                throw new UsuarioException("Username y o Password incorrectos");
            }
            if(!BCrypt.Net.BCrypt.Verify(iniciarSesionDTO.UPassword, encontrarUsuario.UPassword))
            {
                throw new UsuarioException("Username y o Password incorrectos");
            }

            var datosUsuario = UsuarioMapper.UsuarioToDatosUsuario(encontrarUsuario);
            var token = GenerarToken(encontrarUsuario.Id.ToString(), encontrarUsuario.Username);

            return (token, datosUsuario);
        }

        private string GenerarToken(string usuarioID, string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("usuarioid", usuarioID),
                new Claim("username", username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWTConfig:Secret")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<DatosUsuarioDTO> EditarDatosUsuario(String username, EditarUsuarioDTO editarUsuarioDTO)
        {
            editarUsuarioDTO.Nombre = editarUsuarioDTO.Nombre?.Trim();
            editarUsuarioDTO.Apellido = editarUsuarioDTO.Apellido?.Trim();
            editarUsuarioDTO.Email = editarUsuarioDTO.Email?.Trim();
            editarUsuarioDTO.Bio = editarUsuarioDTO.Bio?.Trim();

            ValidarUsuarioPatch.Validar(editarUsuarioDTO);
            var encontrarUsuario = await _usuariosRepositorio.GetUsuarioUsername(username);

            if(encontrarUsuario == null)
            {
                throw new UsuarioException($"Usuario no encontrado por username: {username}");
            }

            try
            {
                encontrarUsuario.Nombre = editarUsuarioDTO.Nombre;
                encontrarUsuario.Apellido = editarUsuarioDTO.Apellido;
                encontrarUsuario.Email = editarUsuarioDTO.Email;
                encontrarUsuario.Bio = editarUsuarioDTO.Bio;
                await _usuariosRepositorio.GuardarCambios();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                if (sqlEx.Message.Contains("UQ_Email"))
                {
                    throw new UsuarioException("El Email ya fue registrado antes");
                }
                throw ex;
            }

            return UsuarioMapper.UsuarioToDatosUsuario(encontrarUsuario);
        }


        public async Task<PerfilUsuarioDTO> GetPerfilUsuario(string username, Guid? usuarioLogId)
        {
            // Busco usuario por username
            var encontrarUsuario = await _usuariosRepositorio.GetUsuarioUsername(username);
            if(encontrarUsuario == null)
            {
                throw new UsuarioException("Usuario no encontrado");
            }
            // Si encuentra usuario, busca sus respuestas
            var encontrarRespuestas = await _interaccionesService.GetRespuestasUsername(username, usuarioLogId);

            // Creo el Perfil DTO e ingreso las respuestas al perfil
            var PerfilUsDTO = UsuarioMapper.UsuarioToPerfilUsuario(encontrarUsuario);
            PerfilUsDTO.LiRespuestas = encontrarRespuestas;

            return PerfilUsDTO;
        }

        public async Task<IEnumerable<UsuarioListaDTO>> BuscarUsuarios(string busqueda)
        {
            var listaUsuariosDB = await _usuariosRepositorio.GetUsuarios(busqueda);
            return listaUsuariosDB.Select(u => UsuarioMapper.UsuarioToUsuarioListaDTO(u));
        }
    }
}
