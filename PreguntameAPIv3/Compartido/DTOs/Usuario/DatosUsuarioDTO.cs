namespace PreguntameAPIv3.Compartido.DTOs.Usuario
{
    public class DatosUsuarioDTO
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string? Apellido { get; set; }

        public string? FotoPath { get; set; }

        public string? FondoPath { get; set; }

        public string? PaisNombre { get; set; }

        public string? Bio { get; set; }
    }
}
