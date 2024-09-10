using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.LogicaAccesoDatos.Models;

namespace PreguntameAPIv3.LogicaNegocio.IRepositorios
{
    public interface IInteraccionesRepositorio
    {
        Task GuardarCambios();
        Task<List<Respuesta>> GetRespuestasUsername(string username);
        Task<List<Pregunta>> GetPreguntasUsername(string username);
        Task<Pregunta> GetPreguntaPorIdyUsername(string username, string preguntaid);
        Task InsertRespuesta(Respuesta respuesta, Pregunta pregunta);
        Task InsertPregunta(Pregunta pregunta);
        Task ToggleLike(Like like);
    }
}
