using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Uteis;
using Domain.Entities.ViewModel;
using Domain.Interfaces;
using Domain.Requests;
using Domain.ViewModel;
using Humanizer;
using Microsoft.AspNetCore.Mvc;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly ILogger<EstoqueController> _logger;
        private readonly IProduto _produto;
        private readonly IEstoque _estoque;
        public EstoqueController( ILogger<EstoqueController> logger, IProduto produto, IEstoque estoque)
        {
            _logger = logger;
            _produto = produto;
            _estoque = estoque;
        }
        #region Estoque
        [HttpGet("Estoques")]
        public async Task<IActionResult> Estoques()
        {
            try
            {
                var estoques = await _estoque.PesquisarTodosEstoques();
                return Ok(new { Estoques = estoques });
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
        [HttpGet("PesquisarEstoquesSemRevendedor")]
        public async Task<IActionResult> PesquisarEstoquesSemRevendedor(int? nCdRevendedor = null)
        {
            try
            {
                var estoques = await _estoque.PesquisarEstoques(nCdRevendedor);
                return Ok(new { Estoques = estoques });
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
        [HttpGet("Estoque/{codigoEstoque}")]
        public async Task<IActionResult> Estoque(int codigoEstoque)
        {
            try
            {
                DTOEstoque oDTOEstoque = await _estoque.Consultar(codigoEstoque);
                return Ok(new { success = true, estoque = oDTOEstoque });
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


        [HttpGet("EstoqueProduto/{codigoEstoque}")]
        public async Task<IActionResult> EstoqueProduto(int codigoEstoque)
        {
            try
            {
                List<DTOEstoqueProdutoHistorico> lstHistoricoEstoque = await _estoque.ConsultarHistorico(codigoEstoque);
                List<DTOEstoqueProduto> estoqueProduto = await _estoque.PesquisarEstoqueProduto(codigoEstoque);
                List<DTOProduto> todosProdutos = await _produto.PesquisarTodosProdutos();               

                return Ok(new { EstoqueProduto = estoqueProduto, Produtos = todosProdutos, Historico = lstHistoricoEstoque });
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
        [HttpGet("HistoricoMovimentacao/{codigoEstoque}")]
        public async Task<IActionResult> HistoricoMovimentacao(int codigoEstoque)
        {
            try
            {
                return Ok(new { Historico = await _estoque.ConsultarHistorico(codigoEstoque) });
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
        [HttpPost("CadastrarEstoque")]
        public async Task<IActionResult> CadastrarEstoque([FromBody] DTOEstoque estoque)
        {
            try
            {
                return Ok(await _estoque.CadastrarEstoque(estoque));
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
        [HttpPost("MovimentarEntradaSaida")]
        public async Task<IActionResult> MovimentarEntradaSaida([FromBody] DTOEstoqueProdutoHistorico oDTOEstoqueProdutoHistorico)
        {
            try
            {
                return Ok(await _estoque.MovimentarEntradaSaida(oDTOEstoqueProdutoHistorico));
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
        [HttpPut("EditarProdutoEstoque")]
        public async Task<IActionResult> EditarProdutoEstoque([FromBody] DTOEstoqueProduto oDTOEstoqueProduto)
        {
            try
            {
                return Ok(await _estoque.AdicionarEditarProdutoEstoque(oDTOEstoqueProduto));
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
        [HttpDelete("RemoverEstoqueProduto")]
        public async Task<IActionResult> RemoverEstoqueProduto([FromBody] EstoqueRequest request)
        {
            try
            {
                return Ok(await _estoque.RemoverEstoqueProduto(request.codigoEstoque, request.arrCodigosProdutos));
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
        [HttpDelete("ExcluirEstoques")]
        public async Task<IActionResult> ExcluirEstoques([FromBody] string arrCodigoEstoques)
        {
            try
            {
                return Ok(await _estoque.ExcluirEstoques(arrCodigoEstoques));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar estoque");
                return StatusCode(500, new { success = false, message = "Ocorreu um erro ao processar sua solicitação." });
            }
        }
        #endregion
    }
}