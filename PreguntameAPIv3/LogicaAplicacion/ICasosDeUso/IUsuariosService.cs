using PreguntameAPIv3.Compartido.DTOs.Usuario;

namespace PreguntameAPIv3.LogicaAplicacion.ICasosDeUso
{
    public interface IUsuariosService
    {
        Task RegistrarUsuario(UsuarioInsertDTO usuarioInsertDTO);
        Task<(string, DatosUsuarioDTO)> IniciarSesion(IniciarSesionDTO iniciarSesionDTO);
        Task<DatosUsuarioDTO> EditarDatosUsuario(String username, EditarUsuarioDTO editarUsuarioDTO);
        Task<PerfilUsuarioDTO> GetPerfilUsuario(string username, Guid? usuarioLogId);
        Task<IEnumerable<UsuarioListaDTO>> BuscarUsuarios(string busqueda);
    }
}
