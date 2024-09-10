using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.Compartido.DTOs.Usuario;
using PreguntameAPIv3.LogicaAccesoDatos.Models;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;

namespace PreguntameAPIv3.Compartido.Mappers
{
    public class UsuarioMapper
    {
        public static Usuario InsertToUsuario(UsuarioInsertDTO usuarioInsertDTO)
        {
            if(usuarioInsertDTO == null)
            {
                throw new UsuarioException("Datos de inserción del usuario vacíos");
            }
            return new Usuario
            {
                Username = usuarioInsertDTO.Username,
                Email = usuarioInsertDTO.Email,
                UPassword = usuarioInsertDTO.UPassword,
                Nombre = usuarioInsertDTO.Nombre,
                Apellido = usuarioInsertDTO.Apellido,
                PaisId = usuarioInsertDTO.PaisId is not null ? new Guid(usuarioInsertDTO.PaisId) : null
            };
        }

        public static DatosUsuarioDTO UsuarioToDatosUsuario(Usuario usuario)
        {
            if(usuario == null)
            {
                throw new UsuarioException("Datos de Usuario inexistentes");
            }
            return new DatosUsuarioDTO
            {
                Username = usuario.Username,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Bio = usuario.Bio,
                FondoPath = usuario.FondoPath,
                FotoPath = usuario.FotoPath,
                PaisNombre = usuario.Pais?.Nombre
            };
        }

        public static PerfilUsuarioDTO UsuarioToPerfilUsuario(Usuario usuario)
        {
            if(usuario == null)
            {
                throw new UsuarioException("Datos de Usuario inexistentes");
            }
            return new PerfilUsuarioDTO
            {
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Bio = usuario.Bio,
                PreguntaBio = usuario.PreguntaBio,
                FotoPath = usuario.FotoPath,
                FondoPath = usuario.FondoPath,
                NombrePais = usuario.Pais?.Nombre,
                NLikes = usuario.NLikes,
                NSeguidores = usuario.NSeguidores
            };
        }

        public static UsuarioListaDTO UsuarioToUsuarioListaDTO(Usuario usuario)
        {
            if( usuario == null)
            {
                throw new UsuarioException("Datos de Usuario inexistentes");
            }
            return new UsuarioListaDTO
            {
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                PaisNombre = usuario.Pais?.Nombre
            };
        }
    }
}
