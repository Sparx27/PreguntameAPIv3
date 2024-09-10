using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;

namespace PreguntameAPIv3.LogicaNegocio.Validadores.Interacciones
{
    public static class ValidarPreguntaInsert
    {
        public static void Validar(PreguntaInsertDTO preguntaInsertDTO)
        {
            if(preguntaInsertDTO == null)
            {
                throw new PreguntaException("Los datos de la Pregunta se encuentran vacíos");
            }

            // Referencias a Usuarios
            if(preguntaInsertDTO.UsuarioRecibe == null)
            {
                throw new PreguntaException("Falta la referencia al Usuario que se intenta enviar la Pregunta");
            }
            if(RegexGenerales.ValidarCaracteresProhibidos(preguntaInsertDTO.UsuarioRecibe))
            {
                throw new PreguntaException("Los caracteres (' \" \\ ; | < > -) están prohibidos");
            }
            if(preguntaInsertDTO.UsuarioEnvia != null)
            {
                if(RegexGenerales.ValidarCaracteresProhibidos(preguntaInsertDTO.UsuarioEnvia)) {
                    throw new PreguntaException("Los caracteres (' \" \\ ; | < > -) están prohibidos");
                }
            }

            // Cuerpo de la pregunta
            if(string.IsNullOrEmpty(preguntaInsertDTO.Dsc) || preguntaInsertDTO.Dsc.Length < 7)
            {
                throw new PreguntaException("El cuerpo de la Pregunta debería contener al menos 7 caracteres");
            }
            if(RegexGenerales.ValidarCaracteresProhibidos(preguntaInsertDTO.Dsc))
            {
                throw new PreguntaException("El cuerpo de la Pregunta no puede contener los caracteres (' \" \\ ; | < > -)");
            }
        }
    }
}
