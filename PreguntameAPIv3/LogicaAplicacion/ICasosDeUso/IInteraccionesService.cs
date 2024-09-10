using PreguntameAPIv3.Compartido.DTOs.Interacciones;

namespace PreguntameAPIv3.LogicaAplicacion.ICasosDeUso
{
    public interface IInteraccionesService
    {
        Task<IEnumerable<RespuestaDTO>> GetRespuestasUsername(string username, Guid? usuarioLogId);
        Task<IEnumerable<PreguntaDTO>> GetPreguntasUsername(string username);
        Task<PreguntaDTO> GetPreguntaPorIdyUsername(string username, string preguntaid);
        Task InsertRespuesta(string username, RespuestaInsertDTO respuestaInsertDTO);
        Task InsertPregunta(PreguntaInsertDTO preguntaInsertDTO);
        Task ToggleLike(LikeInsertDTO likeInsertDTO);
    }
}
