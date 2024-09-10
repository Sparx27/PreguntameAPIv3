namespace PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades
{
    public class PreguntaException : Exception
    {
        public PreguntaException() { }
        public PreguntaException(string message) : base(message) { }
        public PreguntaException(string message,  Exception innerException) : base(message, innerException) { }
    }
}
