using PreguntameAPIv3.LogicaAccesoDatos.Models;

namespace PreguntameAPIv3.Compartido.DTOs.Usuario
{
    public class UsuarioInsertDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? UPassword { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? PaisId { get; set; }
    }
}
