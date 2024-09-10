using PreguntameAPIv3.LogicaAccesoDatos.Models;

namespace PreguntameAPIv3.Compartido.DTOs.Interacciones
{
    public class PreguntaInsertDTO
    {
        public string? UsuarioRecibe { get; set; }
        public string? UsuarioEnvia { get; set; }
        public string? Dsc { get; set; }
    }
}
