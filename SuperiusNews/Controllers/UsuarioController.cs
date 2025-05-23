using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Uteis;
using Domain.ViewModel;
using Domain.Interfaces;
using Domain.Requests;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuario _usuario;
        public UsuarioController(ILogger<UsuarioController> logger, IUsuario usuario)
        {
            _logger = logger;
            _usuario = usuario;
        }
        [HttpGet("Usuario")]
        public async Task<IActionResult> PesquisarUsuarios()
        {
            try
            {
                return Ok( await _usuario.PesquisarUsuarios());
            }
            catch (ExceptionCustom ex)
            {
                return NotFound(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpGet("FuncionalidadesUsuario/{codigoUsuario}")]
        public async Task<IActionResult> FuncionalidadesUsuario(string codigoUsuario)
        {
            try
            {
                return Ok( await _usuario.FuncionalidadesUsuario(codigoUsuario));
            }
            catch (ExceptionCustom ex)
            {
                return NotFound(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpGet("PerfisAssociadosCompleto/{codigoUsuario}")]
        public async Task<IActionResult> PerfisAssociadosCompleto(string codigoUsuario)
        {
            try
            {
                return Ok(  new { perfisAtrelados = await _usuario.PerfisAssociados(codigoUsuario) , perfis = await _usuario.PesquisarPerfisAtivos() });
            }
            catch (ExceptionCustom ex)
            {
                return NotFound(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpGet("PerfisAssociados/{codigoUsuario}")]
        public async Task<IActionResult> PerfisAssociados(string codigoUsuario)
        {
            try
            {
                return Ok( await _usuario.PerfisAssociados(codigoUsuario));
            }
            catch (ExceptionCustom ex)
            {
                return NotFound(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpPost("Usuario")]
        public async Task<IActionResult> Usuario(DTOUsuario oDTOUsuario)
        {
            try
            {
                return Ok( await _usuario.CadastrarUsuario(oDTOUsuario));
            }
            catch (ExceptionCustom ex)
            {
                return NotFound(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpPost("AssociarDesassociarPerfis")]
        public async Task<IActionResult> AssociarDesassociarPerfis(AssociacaoUsuarioRequest associacaoRequest)
        {
            try
            {
                return Ok( await _usuario.AssociarDesassociarPerfis(associacaoRequest));
            }
            catch (ExceptionCustom ex)
            {
                return NotFound(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpPost("AssociarPerfis")]
        public async Task<IActionResult> AssociarPerfis(AssociacaoUsuarioRequest associacaoRequest)
        {
            try
            {
                return Ok( await _usuario.AssociarPerfis(associacaoRequest));
            }
            catch (ExceptionCustom ex)
            {
                return NotFound(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpPost("DesassociarPerfis")]
        public async Task<IActionResult> DesassociarPerfis(AssociacaoUsuarioRequest associacaoRequest)
        {
            try
            {
                return Ok( await _usuario.DesassociarPerfis(associacaoRequest));
            }
            catch (ExceptionCustom ex)
            {
                return NotFound(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpPut("Usuario")]
        public async Task<IActionResult> EditarUsuario(DTOUsuario oDTOUsuario)
        {
            try
            {
                return Ok( await _usuario.EditarUsuario(oDTOUsuario));
            }
            catch (ExceptionCustom ex)
            {
                return NotFound(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
    }
}