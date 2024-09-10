using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;

namespace PreguntameAPIv3.LogicaNegocio.Validadores.Interacciones
{
    public static class ValidarRespuestaInsert
    {
        public static void Validar(RespuestaInsertDTO respuestaInsertDTO)
        {
            if (respuestaInsertDTO == null)
            {
                throw new RespuestaException("Los datos de Respuesta se encuentran vacíos");
            }

            // PreguntaID
            if(string.IsNullOrEmpty(respuestaInsertDTO.PreguntaId))
            {
                throw new RespuestaException("Falta la referencia a la Pregunta en Respuesta");
            }

            // Dsc
            if(string.IsNullOrEmpty(respuestaInsertDTO.Dsc) || respuestaInsertDTO.Dsc.Length < 1)
            {
                throw new RespuestaException("El cuerpo de la Respuesta debe contener al menos 1 caracter");
            }
            if(RegexGenerales.ValidarCaracteresProhibidos(respuestaInsertDTO.Dsc))
            {
                throw new RespuestaException("El cuerpo de la Respuesta no puede contener los caracteres (' \" \\ ; | < > -)");
            }
        }
    }
}
