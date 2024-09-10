using PreguntameAPIv3.Compartido.DTOs.Usuario;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;

namespace PreguntameAPIv3.LogicaNegocio.Validadores.Usuario
{
    public static class ValidarUsuarioPatch
    {
        public static void Validar(EditarUsuarioDTO editarUsuarioDTO)
        {
            var (nombre, apellido, email, bio) = (editarUsuarioDTO.Nombre, editarUsuarioDTO.Apellido, editarUsuarioDTO.Email, editarUsuarioDTO.Bio);

            // Nombre
            if (String.IsNullOrEmpty(nombre))
            {
                throw new UsuarioException("Nombre es obligatorio");
            }
            if (RegexGenerales.ValidarCaracteresProhibidos(nombre))
            {
                throw new UsuarioException("Los caracteres (' \" \\ ; | < > -) estan prohibidos");
            }
            if (nombre.Length < 2 || nombre.Length > 30)
            {
                throw new UsuarioException("Nombre debe contener entre 2 y 30 caracteres");
            }
            if (!RegexGenerales.ValidarSoloAlfabeticos(nombre))
            {
                throw new UsuarioException("Nombre debería contener solamente caracteres alfabéticos");
            }
            //Apellido
            if (!String.IsNullOrEmpty(apellido))
            {
                if (apellido.Length < 2 || apellido.Length > 40)
                {
                    throw new UsuarioException("Apellido no es obligatorio, pero debería contener entre 2 y 40 caracteres");
                }
                if (RegexGenerales.ValidarCaracteresProhibidos(apellido))
                {
                    throw new UsuarioException("Los caracteres (' \" \\ ; | < > -) estan prohibidos");
                }
                if (!RegexGenerales.ValidarSoloAlfabeticos(apellido))
                {
                    throw new UsuarioException("Apellido debería contener solamente caracteres alfabéticos");
                }
            }

            // Email
            if (String.IsNullOrEmpty(email))
            {
                throw new UsuarioException("Email es obligatorio");
            }
            if (RegexGenerales.ValidarCaracteresProhibidos(email))
            {
                throw new UsuarioException("Email no puede contener los caracteres (' \" \\ ; | < > -)");
            }
            if (!RegexGenerales.ValidarFormatoEmail(email))
            {
                throw new UsuarioException("El formato de Email es incorrecto");
            }

            // Bio
            //Apellido
            if (!String.IsNullOrEmpty(bio))
            {
                if (RegexGenerales.ValidarCaracteresProhibidos(bio))
                {
                    throw new UsuarioException("Bio: Los caracteres (' \" \\ ; | < > -) estan prohibidos");
                }
                if (bio.Length > 150)
                {
                    throw new UsuarioException("Bio no puede contener más de 150 caracteres");
                }
            }

        }
    }
}
