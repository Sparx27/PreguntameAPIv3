namespace PreguntameAPIv3.Compartido.DTOs.Usuario
{
    public class UsuarioListaDTO
    {
        public string Username { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Apellido { get; set; }
        public string? PaisNombre { get; set; }
    }
}
