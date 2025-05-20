
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
    public class PerfilController : ControllerBase
    {
        private readonly ILogger<PerfilController> _logger;
        private readonly IPerfil _perfil;
        public PerfilController(ILogger<PerfilController> logger, IPerfil Perfil)
        {
            _logger = logger;
            _perfil = Perfil;
        }
        [HttpGet("Perfil")]
        public async Task<IActionResult> PesquisarPerfil()
        {
            try
            {
                return Ok( await _perfil.PesquisarPerfis());
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
        [HttpGet("PermissoesAssociadas/{codigoPerfil}")]
        public async Task<IActionResult> PermissoesAssociadas(int codigoPerfil)
        {
            try
            {
                return Ok( await _perfil.PermissoesAssociadas(codigoPerfil));
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
        [HttpPost("Perfil")]
        public async Task<IActionResult> Perfil(DTOPerfil oDTOPerfil)
        {
            try
            {
                return Ok( await _perfil.CadastrarPerfil(oDTOPerfil));
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
        [HttpPost("AtivarPerfil")]
        public async Task<IActionResult> AtivarPerfis([FromBody] string arrCodigoPerfis)
        {
            try
            {
                return Ok( await _perfil.AtivarPerfis(arrCodigoPerfis));
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
        [HttpPost("InativarPerfil")]
        public async Task<IActionResult> InativarPerfis([FromBody] string arrCodigoPerfis)
        {
            try
            {
                return Ok( await _perfil.InativarPerfis(arrCodigoPerfis));
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
        [HttpPost("AssociarPermissoes")]
        public async Task<IActionResult> AssociarPermissoes(AssociacaoRequest associacaoRequest)
        {
            try
            {
                return Ok( await _perfil.AssociarPermissoes(associacaoRequest));
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
        [HttpPost("DesassociarPermissoes")]
        public async Task<IActionResult> DesassociarPermissoes(AssociacaoRequest associacaoRequest)
        {
            try
            {
                return Ok( await _perfil.DesassociarPermissoes(associacaoRequest));
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
        [HttpPut("Perfil")]
        public async Task<IActionResult> EditarPerfil(DTOPerfil oDTOPerfil)
        {
            try
            {
                return Ok( await _perfil.EditarPerfil(oDTOPerfil));
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