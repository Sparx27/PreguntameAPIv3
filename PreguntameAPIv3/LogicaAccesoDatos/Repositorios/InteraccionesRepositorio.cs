using Microsoft.EntityFrameworkCore;
using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.LogicaAccesoDatos.Models;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;
using PreguntameAPIv3.LogicaNegocio.IRepositorios;

namespace PreguntameAPIv3.LogicaAccesoDatos.Repositorios
{
    public class InteraccionesRepositorio : IInteraccionesRepositorio
    {
        private PreguntameDBContext _context;
        public InteraccionesRepositorio(PreguntameDBContext context)
        {
            _context = context;
        }

        public async Task GuardarCambios()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<Respuesta>> GetRespuestasUsername(string username) =>
            await _context.Respuestas
                .Where(r => r.Pregunta.UsuarioRecibe == username)
                .Include(r => r.Pregunta)
                .Include(r => r.Likes)
                .ToListAsync();

        public async Task<List<Pregunta>> GetPreguntasUsername(string username) =>
            await _context.Preguntas
                .Where(p => p.UsuarioRecibe == username && p.Respondida == false)
                .ToListAsync();

        public async Task<Pregunta> GetPreguntaPorIdyUsername(string username, string preguntaid) =>
            await _context.Preguntas.FirstOrDefaultAsync(p => p.Respondida == false && p.UsuarioRecibe == username && p.Id.ToString() == preguntaid);

        public async Task InsertRespuesta(Respuesta respuesta, Pregunta pregunta)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                await _context.Respuestas.AddAsync(respuesta);

                pregunta.Respondida = true;

                await _context.SaveChangesAsync();
                transaccion.Commit();
            }
        }

        public async Task InsertPregunta(Pregunta pregunta)
        {
            await _context.Preguntas.AddAsync(pregunta);
            await _context.SaveChangesAsync();
        }

        public async Task ToggleLike(Like like)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == like.IdUsuario);
            if (usuario == null)
            {
                throw new LikeException("No se encontró al Usuario que intenta dar Like a la Respuesta");
            }

            var respuesta = await _context.Respuestas.FirstOrDefaultAsync(a => a.Id == like.IdRespuesta);
            if (respuesta == null)
            {
                throw new LikeException("No se encontró la Pregunta a la que se intenta dar Like");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var yaSeDioLike = await _context.Likes.FirstOrDefaultAsync(l => l.IdRespuesta == like.IdRespuesta && l.IdUsuario == like.IdUsuario);

                if(yaSeDioLike == null)
                {
                    await _context.AddAsync(like);
                    usuario.NLikes++;
                    respuesta.NLikes++;
                }

                if(yaSeDioLike != null)
                {
                    _context.Likes.Remove(yaSeDioLike);
                    usuario.NLikes--;
                    respuesta.NLikes--;
                }

                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}
