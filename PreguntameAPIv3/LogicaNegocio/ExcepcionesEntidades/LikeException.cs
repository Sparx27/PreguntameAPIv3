namespace PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades
{
    public class LikeException : Exception
    {
        public LikeException() { }
        public LikeException(string message) : base(message) { }
        public LikeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
