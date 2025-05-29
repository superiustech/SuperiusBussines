using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Interfaces;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;


using Domain.Entities.Uteis;
using Domain.ViewModel;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevendedorController : ControllerBase
    {
        private readonly ILogger<RevendedorController> _logger;
        private readonly IRevendedor _revendedor;
        private readonly IUsuario _usuario;
        public RevendedorController( ILogger<RevendedorController> logger, IRevendedor revendedor, IUsuario usuario)
        {
            _logger = logger;
            _revendedor = revendedor;
            _usuario = usuario;
        }

        [HttpGet("TiposRevendedor")]
        public async Task<IActionResult> TiposRevendedor()
        {
            try
            {
                return Ok( new { tipos = await _revendedor.PesquisarTipos() });
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
        [HttpGet("Revendedor/{nCdRevendedor}")]
        public async Task<IActionResult> Revendedor(int nCdRevendedor)
        {
            try 
            {
                DTORevendedor oDTORevendedor = await _revendedor.Consultar(nCdRevendedor);
                return Ok(new { revendedor = oDTORevendedor });
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
        [HttpGet("Revendedores/")]
        public async Task<IActionResult> Revendedores()
        {
            try
            { 
                return Ok(new {revendedores = await _revendedor.PesquisarRevendedores() });
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
        [HttpPost("CadastrarRevendedor")]
        public async Task<IActionResult> CadastrarRevendedor([FromBody] DTORevendedor oDTORevendedor)
        {
            try
            {
                return Ok(await _revendedor.CadastrarRevendedor(oDTORevendedor));
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
        [HttpPost("AssociarDesassociarUsuarios")]
        public async Task<IActionResult> AssociarDesassociarUsuarios(AssociacaoRevendedorUsuarioRequest associacaoRequest)
        {
            try
            {
                return Ok( await _revendedor.AssociarDesassociarUsuarios(associacaoRequest));
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
        [HttpGet("UsuariosAssociadosCompleto/{codigoRevendedor}")]
        public async Task<IActionResult> UsuariosAssociadosCompleto(int codigoRevendedor)
        {
            try
            {
                return Ok(new { usuariosAtrelados = await _revendedor.UsuariosAssociados(codigoRevendedor) , usuarios = await _usuario.PesquisarUsuarios() });
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
        [HttpDelete("ExcluirRevendedores")]
        public async Task<IActionResult> ExcluirRevendedores([FromBody] string arrCodigoRevendedores)
        {
            try
            {
                return Ok(await _revendedor.ExcluirRevendedores(arrCodigoRevendedores));
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