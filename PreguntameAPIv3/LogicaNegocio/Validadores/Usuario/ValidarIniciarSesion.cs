using PreguntameAPIv3.Compartido.DTOs.Usuario;
using System.Text.RegularExpressions;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;

namespace PreguntameAPIv3.LogicaNegocio.Validadores.Usuario
{
    public static class ValidarIniciarSesion
    {
        public static void Validar(IniciarSesionDTO iniciarSesionDTO)
        {
            var (username, uPassword) = (iniciarSesionDTO.Username, iniciarSesionDTO.UPassword);

            // Verificar caracteres prohibidos
            if(username != null )
            {
                if(RegexGenerales.ValidarCaracteresProhibidos(username))
                {
                    throw new UsuarioException("Los caracteres (' \" \\ ; | < > -) estan prohibidos");
                }
            }
            if (uPassword != null )
            {
                if (RegexGenerales.ValidarCaracteresProhibidos(uPassword))
                {
                    throw new UsuarioException("Los caracteres (' \" \\ ; | < > -) estan prohibidos");
                }
            }

            // Campos vacíos
            if(username == null)
            {
                throw new UsuarioException("Falta el username");
            }
            if(uPassword == null)
            {
                throw new UsuarioException("Falta el password");
            }
        }
    }

    


}
