using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.LogicaAccesoDatos.Models;

namespace PreguntameAPIv3.Compartido.DTOs.Usuario
{
    public class PerfilUsuarioDTO
    {
        public string Username { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Apellido { get; set; }
        public string? Bio { get; set; }
        public string? PreguntaBio { get; set; }
        public string? FotoPath { get; set; }
        public string? FondoPath { get; set; }
        public string? NombrePais { get; set; }
        public int NLikes { get; set; }
        public int NSeguidores { get; set; }
        public IEnumerable<RespuestaDTO> LiRespuestas { get; set; } = Enumerable.Empty<RespuestaDTO>();
    }
}
