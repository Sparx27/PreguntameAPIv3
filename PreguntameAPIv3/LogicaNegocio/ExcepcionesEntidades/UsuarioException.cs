namespace PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades
{
    public class UsuarioException : Exception
    {
        public UsuarioException() { }
        public UsuarioException(string message) : base(message) { }
        public UsuarioException(string message, Exception innerException) : base(message, innerException) { }
    }
}
