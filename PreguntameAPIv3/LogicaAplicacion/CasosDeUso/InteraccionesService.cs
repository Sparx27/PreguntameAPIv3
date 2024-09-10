using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.Compartido.Mappers;
using PreguntameAPIv3.LogicaAccesoDatos.Models;
using PreguntameAPIv3.LogicaAplicacion.ICasosDeUso;
using PreguntameAPIv3.LogicaNegocio.IRepositorios;
using PreguntameAPIv3.LogicaNegocio.ExcepcionesEntidades;
using PreguntameAPIv3.LogicaNegocio.Validadores.Interacciones;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace PreguntameAPIv3.LogicaAplicacion.CasosDeUso
{
    public class InteraccionesService : IInteraccionesService
    {
        private IInteraccionesRepositorio _intRepo;
        public InteraccionesService(IInteraccionesRepositorio intRepo)
        {
            _intRepo = intRepo;
        }

        public async Task<IEnumerable<RespuestaDTO>> GetRespuestasUsername(string username, Guid? usuarioLogId)
        {
            var respuestasDB = await _intRepo.GetRespuestasUsername(username);
            return respuestasDB.Select(r => InteraccionesMapper.RespuestaToRespuestaDTO(r, usuarioLogId));
        }
        public async Task<IEnumerable<PreguntaDTO>> GetPreguntasUsername(string username)
        {
            var preguntasDB = await _intRepo.GetPreguntasUsername(username);
            return preguntasDB.Select(p => InteraccionesMapper.PreguntaToPreguntaDTO(p));
        }

        public async Task<PreguntaDTO> GetPreguntaPorIdyUsername(string username, string preguntaid)
        {
            var preguntaDB = await _intRepo.GetPreguntaPorIdyUsername(username, preguntaid);
            if(preguntaDB == null)
            {
                throw new PreguntaException("La pregunta no existe o ya fue respondida");
            }
            return InteraccionesMapper.PreguntaToPreguntaDTO(preguntaDB);
        }

        public async Task InsertRespuesta(string username, RespuestaInsertDTO respuestaInsertDTO)
        {
            var verificarSiPreguntaEsDeUsuario = await _intRepo.GetPreguntaPorIdyUsername(username, respuestaInsertDTO.PreguntaId);
            if(verificarSiPreguntaEsDeUsuario == null)
            {
                throw new RespuestaException("La pregunta que intenta responder, no pertenece a su usuario");
            }

            respuestaInsertDTO.Dsc = respuestaInsertDTO.Dsc?.Trim();
            ValidarRespuestaInsert.Validar(respuestaInsertDTO);

            try
            {
                await _intRepo.InsertRespuesta(InteraccionesMapper.RespuestaInsertDTOtoRespuesta(respuestaInsertDTO), verificarSiPreguntaEsDeUsuario);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                if (sqlEx.Message.Contains("UQ_PreguntaIDRespuestas"))
                {
                    throw new RespuestaException("Esta Pregunta ya ha sido respondida antes");
                }
                throw ex;
            }

        }

        public async Task InsertPregunta(PreguntaInsertDTO preguntaInsertDTO)
        {
            preguntaInsertDTO.Dsc = preguntaInsertDTO.Dsc?.Trim();
            ValidarPreguntaInsert.Validar(preguntaInsertDTO);

            await _intRepo.InsertPregunta(InteraccionesMapper.PreguntaInsertDTOtoPregunta(preguntaInsertDTO));
        }

        public async Task ToggleLike(LikeInsertDTO likeInsertDTO)
        {
            
            if (String.IsNullOrEmpty(likeInsertDTO.IdRespuesta) || String.IsNullOrEmpty(likeInsertDTO.IdUsuario))
            {
                throw new LikeException("Falta el ID de la Respuesta o el ID del Usuario en el envío de datos de Like");
            }
            await _intRepo.ToggleLike(InteraccionesMapper.LikeInsertDTOtoLike(likeInsertDTO));
        }
    }
}
