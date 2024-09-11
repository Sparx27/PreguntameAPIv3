using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.LogicaAccesoDatos.Models;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;

namespace PreguntameAPIv3.Compartido.Mappers
{
    public static class InteraccionesMapper
    {
        public static RespuestaDTO RespuestaToRespuestaDTO(Respuesta respuesta, Guid? usuarioLogId)
        {
            if (respuesta == null)
            {
                throw new RespuestaException("Datos de Respuesta vacíos en Mapper");
            }
            return new RespuestaDTO
            {
                RespuestaID = respuesta.Id.ToString(),
                DscRespuesta = respuesta.Dsc,
                FechaRespuesta = respuesta.Fecha,
                Nlikes = respuesta.NLikes,
                DscPregunta = respuesta.Pregunta.Dsc,
                PreguntaAnonima = respuesta.Pregunta.Anonima,
                UsuarioRecibe = respuesta.Pregunta.UsuarioRecibe,
                UsuarioEnvia = respuesta.Pregunta.Anonima ? "Anónimo" : respuesta.Pregunta?.UsuarioEnvia,
                FechaPregunta = respuesta.Pregunta.Fecha,
                LikeUsuarioLog = usuarioLogId is null 
                    ? false 
                    : respuesta.Likes.FirstOrDefault(l => l.IdUsuarioEnvia == usuarioLogId) is null
                        ? false
                        : true
            };
        }

        public static Respuesta RespuestaInsertDTOtoRespuesta(RespuestaInsertDTO respuestaInsertDTO)
        {
            if (respuestaInsertDTO == null)
            {
                throw new RespuestaException("Datos de RespuestaInsertDTO vacíos en Mapper");
            }
            return new Respuesta
            {
                PreguntaId = new Guid(respuestaInsertDTO.PreguntaId),
                Dsc = respuestaInsertDTO.Dsc
            };
        }

        public static PreguntaDTO PreguntaToPreguntaDTO(Pregunta pregunta)
        {
            if(pregunta == null)
            {
                throw new PreguntaException("Datos de Pregunta vacíos en Mapper");
            }
            return new PreguntaDTO
            {
                Id = pregunta.Id,
                UsuarioRecibe = pregunta.UsuarioRecibe,
                Anonima = pregunta.Anonima,
                Dsc = pregunta.Dsc,
                Fecha = pregunta.Fecha,
                UsuarioEnvia = pregunta.UsuarioEnvia
            };
        }

        public static Pregunta PreguntaInsertDTOtoPregunta(PreguntaInsertDTO preguntaInserDTO)
        {
            if(preguntaInserDTO == null)
            {
                throw new PreguntaException("Datos de PreguntaInsertDTO vacíos en Mapper");
            }
            return new Pregunta
            {
                UsuarioRecibe = preguntaInserDTO.UsuarioRecibe,
                UsuarioEnvia = preguntaInserDTO.UsuarioEnvia,
                Anonima = preguntaInserDTO.UsuarioEnvia is null ? true : false,
                Dsc = preguntaInserDTO.Dsc
            };
        }

        public static Like LikeInsertDTOtoLike(LikeInsertDTO likeInsertDTO)
        {
            if (likeInsertDTO == null)
            {
                throw new LikeException("Datos de LikeInsertDTO vacíos en Mapper");
            }
            return new Like
            {
                IdRespuesta = new Guid(likeInsertDTO.IdRespuesta),
                IdUsuarioEnvia = new Guid(likeInsertDTO.IdUsuarioEnvia),
                UsernameUsuarioRecibe = likeInsertDTO.UsernameUsuarioRecibe
            };
        }
    }
}
