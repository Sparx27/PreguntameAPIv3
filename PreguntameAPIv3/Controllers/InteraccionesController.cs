using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreguntameAPIv3.Compartido.DTOs.Interacciones;
using PreguntameAPIv3.LogicaAccesoDatos.Models;
using PreguntameAPIv3.LogicaAplicacion.ICasosDeUso;
using System.IdentityModel.Tokens.Jwt;

namespace PreguntameAPIv3.Controllers
{
    [Route("api/interacciones")]
    [ApiController]
    public class InteraccionesController : ControllerBase
    {
        private IInteraccionesService _interaccionesService;
        public InteraccionesController(IInteraccionesService interaccionesService)
        {
            _interaccionesService = interaccionesService;
        }

        [HttpPost("enviar-pregunta")]
        public async Task<IActionResult> EnviarPregunta([FromBody] PreguntaInsertDTO preguntaInsertDTO)
        {
            string? username = null;
            var token = HttpContext.Request.Cookies["jwttoken"];
            if(token != null) 
            { 
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                username = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value;
            }

            if(username != null)
            {
                preguntaInsertDTO.UsuarioEnvia = username;
            }

            try
            {
                await _interaccionesService.InsertPregunta(preguntaInsertDTO);
                return Ok(new { Message = "Pregunta enviada con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("like-pregunta")]
        public async Task<IActionResult> ToggleLikePregunta([FromBody] LikeInsertDTO likeInsertDTO)
        {
            var usuarioId = User.FindFirst("usuarioid")?.Value;
            if (usuarioId == null)
            {
                return BadRequest(new { Message = "No se reconoce al usuario que solicita esta acción" });
            }

            likeInsertDTO.IdUsuario = usuarioId;

            try
            {
                await _interaccionesService.ToggleLike(likeInsertDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
