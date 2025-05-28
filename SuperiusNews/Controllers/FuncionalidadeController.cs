using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Uteis;
using Domain.ViewModel;
using Domain.Interfaces;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionalidadeController : ControllerBase
    {
        private readonly ILogger<FuncionalidadeController> _logger;
        private readonly IFuncionalidade _funcionalidade;
        public FuncionalidadeController( ILogger<FuncionalidadeController> logger, IFuncionalidade funcionalidade)
        {
            _logger = logger;
            _funcionalidade = funcionalidade;
        }
        [HttpGet("Funcionalidades")]
        public async Task<IActionResult> PesquisarFuncionalidade()
        {
            try
            {
                return Ok( await _funcionalidade.PesquisarFuncionalidades());
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
        [HttpPost("Funcionalidade")]
        public async Task<IActionResult> Funcionalidade(DTOFuncionalidade oDTOFuncionalidade)
        {
            try
            {
                return Ok( await _funcionalidade.CadastrarFuncionalidade(oDTOFuncionalidade));
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
        [HttpPost("AtivarFuncionalidade")]
        public async Task<IActionResult> AtivarFuncionalidades([FromBody] string arrCodigoFuncionalidades)
        {
            try
            {
                return Ok( await _funcionalidade.AtivarFuncionalidades(arrCodigoFuncionalidades));
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
        [HttpPost("InativarFuncionalidade")]
        public async Task<IActionResult> InativarFuncionalidades([FromBody] string arrCodigoFuncionalidades)
        {
            try
            {
                return Ok( await _funcionalidade.InativarFuncionalidades(arrCodigoFuncionalidades));
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
        [HttpPut("Funcionalidade")]
        public async Task<IActionResult> EditarFuncionalidade(DTOFuncionalidade oDTOFuncionalidade)
        {
            try
            {
                return Ok( await _funcionalidade.EditarFuncionalidade(oDTOFuncionalidade));
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