
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
    public class PermissaoController : ControllerBase
    {
        private readonly ILogger<PermissaoController> _logger;
        private readonly IPermissao _permissao;
        private readonly IFuncionalidade _funcionalidade;
        public PermissaoController(ILogger<PermissaoController> logger, IPermissao permissao, IFuncionalidade funcionalidade)
        {
            _logger = logger;
            _permissao = permissao;
            _funcionalidade = funcionalidade;
        }
        [HttpGet("Permissoes")]
        public async Task<IActionResult> PesquisarPermissao()
        {
            try
            {
                return Ok( await _permissao.PesquisarPermissoes());
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
        [HttpGet("FuncionalidadesAssociadas/{codigoPermissao}")]
        public async Task<IActionResult> FuncionalidadesAssociadas(int codigoPermissao)
        {
            try
            {
                return Ok( await _permissao.FuncionalidadesAssociadas(codigoPermissao));
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
        [HttpGet("FuncionalidadesAssociadasCompleto/{codigoPermissao}")]
        public async Task<IActionResult> FuncionalidadesAssociadasCompleto(int codigoPermissao)
        {
            try
            {
                return Ok(  new { funcionalidadesAtreladas = await _permissao.FuncionalidadesAssociadas(codigoPermissao) , funcionalidades = await _permissao.PesquisarFuncionalidadesAtivas() });
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
        [HttpPost("Permissao")]
        public async Task<IActionResult> Permissao(DTOPermissao oDTOPermissao)
        {
            try
            {
                return Ok( await _permissao.CadastrarPermissao(oDTOPermissao));
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
        [HttpPost("AssociarFuncionalidades")]
        public async Task<IActionResult> AssociarFuncionalidades(AssociacaoRequest associacaoRequest)
        {
            try
            {
                return Ok( await _permissao.AssociarFuncionalidades(associacaoRequest));
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
        [HttpPost("AssociarDesassociarFuncionalidades")]
        public async Task<IActionResult> AssociarDesassociarFuncionalidades(AssociacaoRequest associacaoRequest)
        {
            try
            {
                return Ok( await _permissao.AssociarDesassociarFuncionalidades(associacaoRequest));
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
        [HttpPost("DesassociarFuncionalidades")]
        public async Task<IActionResult> DesassociarFuncionalidades(AssociacaoRequest associacaoRequest)
        {
            try
            {
                return Ok( await _permissao.DesassociarFuncionalidades(associacaoRequest));
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
        [HttpPost("AtivarPermissoes")]
        public async Task<IActionResult> AtivarPermissoes([FromBody] string arrCodigoPermissoes)
        {
            try
            {
                return Ok( await _permissao.AtivarPermissoes(arrCodigoPermissoes));
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
        [HttpPost("InativarPermissoes")]
        public async Task<IActionResult> InativarPermissoes([FromBody] string arrCodigoPermissoes)
        {
            try
            {
                return Ok( await _permissao.InativarPermissoes(arrCodigoPermissoes));
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
        [HttpPut("Permissao")]
        public async Task<IActionResult> EditarPermissao(DTOPermissao oDTOPermissao)
        {
            try
            {
                return Ok( await _permissao.EditarPermissao(oDTOPermissao));
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