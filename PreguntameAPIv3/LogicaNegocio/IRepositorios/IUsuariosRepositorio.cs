namespace PreguntameAPIv3.LogicaNegocio.IRepositorios
{
    public interface IUsuariosRepositorio<T>
    {
        Task GuardarCambios();
        Task InsertUsuario(T usuario);
        Task<T> GetUsuarioUsername(string username);
        Task<List<T>> GetUsuarios(string busqueda);

    }
}
