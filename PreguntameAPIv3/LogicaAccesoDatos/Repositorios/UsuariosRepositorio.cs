using Microsoft.EntityFrameworkCore;
using PreguntameAPIv3.LogicaAccesoDatos.Models;
using PreguntameAPIv3.LogicaNegocio.IRepositorios;

namespace PreguntameAPIv3.LogicaAccesoDatos.Repositorios
{
    public class UsuariosRepositorio : IUsuariosRepositorio<Usuario>
    {
        private PreguntameDbContext _context;
        public UsuariosRepositorio(PreguntameDbContext context)
        {
            _context = context;
        }

        public async Task GuardarCambios()
        {
            await _context.SaveChangesAsync();
        }

        public async Task InsertUsuario(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> GetUsuarioUsername(string username) => 
            await _context.Usuarios.Include(u => u.Pais).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<List<Usuario>> GetUsuarios(string busqueda) =>
            await _context.Usuarios.Where(u => u.Username.Contains(busqueda) || (u.Nombre + " " + u.Apellido).Contains(busqueda)).ToListAsync();
    }
}
