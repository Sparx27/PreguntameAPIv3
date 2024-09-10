namespace PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades
{
    public class RespuestaException : Exception
    {
        public RespuestaException() { }
        public RespuestaException(string msg) : base(msg) { }
        public RespuestaException(string msg,  Exception inner) : base(msg, inner) { }
    }
}
