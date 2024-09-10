using PreguntameAPIv3.LogicaAccesoDatos.Models;

namespace PreguntameAPIv3.Compartido.DTOs.Interacciones
{
    public class PreguntaDTO
    {
        public Guid Id { get; set; }
        public string UsuarioRecibe { get; set; } = null!;
        public string? UsuarioEnvia { get; set; }
        public bool Anonima { get; set; }
        public string Dsc { get; set; } = null!;
        public DateTime Fecha { get; set; }
    }
}
