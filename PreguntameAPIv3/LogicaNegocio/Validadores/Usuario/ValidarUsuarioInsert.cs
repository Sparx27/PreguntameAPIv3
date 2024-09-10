using PreguntameAPIv3.Compartido.DTOs.Usuario;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;
using System.Text.RegularExpressions;

namespace PreguntameAPIv3.LogicaNegocio.Validadores.Usuario
{
    public static class ValidarUsuarioInsert
    {
        public static void Validar(UsuarioInsertDTO usuarioInsertDTO)
        {
            if(usuarioInsertDTO == null)
            {
                throw new UsuarioException("Los datos de usuario se encuntran vacíos");
            }

            // Spread de valores del usuarioInsertDTO
            var (username, email, uPassword, nombre, apellido, paisId)
                = (usuarioInsertDTO.Username, usuarioInsertDTO.Email, usuarioInsertDTO.UPassword, 
                  usuarioInsertDTO.Nombre, usuarioInsertDTO.Apellido, usuarioInsertDTO.PaisId);

            // Username
            if(String.IsNullOrEmpty(username))
            {
                throw new UsuarioException("Username es obligatorio");
            }
            if (RegexGenerales.ValidarCaracteresProhibidos(username))
            {
                throw new UsuarioException("Username no puede contener los caracteres (' \" \\ ; | < > -)");
            }
            if (username.Length < 3 || username.Length > 20)
            {
                throw new UsuarioException("Username debe contener entre 3 y 20 caracteres");
            }
            if(RegexGenerales.ValidarEspaciosBlanco(username))
            {
                throw new UsuarioException("Username no puede contener espacios en blanco");
            }
            

            // Email
            if(String.IsNullOrEmpty(email))
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

            // UPassword
            if(String.IsNullOrEmpty(uPassword))
            {
                throw new UsuarioException("Contraseña es obligatoria");
            }
            if(uPassword.Length < 6 || uPassword.Length > 20)
            {
                throw new UsuarioException("Contraseña debe contener entre 6 y 30 caracteres");
            }
            if (RegexGenerales.ValidarCaracteresProhibidos(uPassword))
            {
                throw new UsuarioException("Contraseña no puede contener los caracteres (' \" \\ ; | < > -)");
            }
            if (RegexGenerales.ValidarEspaciosBlanco(uPassword))
            {
                throw new UsuarioException("Contraseña no puede contener espacio en blanco");
            }

            // Nombre
            if(String.IsNullOrEmpty(nombre))
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
            if(!RegexGenerales.ValidarSoloAlfabeticos(nombre))
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
        }
    }
}
