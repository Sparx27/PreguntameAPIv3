using PreguntameAPIv3.LogicaAccesoDatos.Models;

namespace PreguntameAPIv3.Compartido.DTOs.Interacciones
{
    public class RespuestaDTO
    {
        public string RespuestaID { get; set; } = null!;
        public string DscRespuesta { get; set; } = null!;
        public DateTime FechaRespuesta { get; set; }
        public int Nlikes { get; set; }
        public string DscPregunta { get; set; } = null!;
        public bool PreguntaAnonima { get; set; }
        public string UsuarioRecibe { get; set; } = null!;
        public string? UsuarioEnvia { get; set; }
        public DateTime FechaPregunta { get; set; }
        public bool LikeUsuarioLog { get; set; } = false;
    }
}
