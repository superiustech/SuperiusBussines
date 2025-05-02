using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Uteis;
using Domain.Entities.ViewModel;
using Domain.Interfaces;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly IProduto _produto;

        public ProdutoController( ILogger<ProdutoController> logger, IProduto produto)
        {
            _logger = logger;
            _produto = produto;
        }

        #region Produto
        [HttpGet("ConsultarProduto/{nCdProduto}")]
        public async Task<IActionResult> ConsultarProduto(int nCdProduto)
        {
            try
            {
                return Ok(new { produto = await _produto.ConsultarProduto(nCdProduto) });
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
        [HttpGet("Produtos")]
        public async Task<IActionResult> Produtos()
        {
            try
            {
                return Ok(new { Produtos = await _produto.PesquisarTodosProdutos() });
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
        [HttpGet("ConsultarVariacoesProduto/{nCdProduto}")]
        public async Task<IActionResult> ConsultarVariacoesProduto(int nCdProduto)
        {
            try
            {
                var variacoes = await _produto.ConsultarProdutoVariacao(nCdProduto);
                return Ok(variacoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
            }
        }

        [HttpGet("TipoVariacao")]
        public async Task<IActionResult> TipoVariacao()
        {
            try
            {
                return Ok(await _produto.ObterVariacoesAtivas());
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

        [HttpGet("UnidadeDeMedida")]
        public async Task<IActionResult> UnidadeDeMedida()
        {
            try
            {
                var unidadeMedidas = await _produto.ObterUnidadesAtivas();
                return Ok(new { success = true, unidade = unidadeMedidas });
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

        [HttpGet("OpcoesVariacao/{tipo}")]
        public async Task<IActionResult> GetOpcoesVariacao(int tipo)
        {
            try
            {
                return Ok(await _produto.ObterVariacoesAtivas(tipo));
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

        [HttpGet("ImagensProduto/{codigoProduto}")]
        public async Task<IActionResult> ObterImagensProduto(int codigoProduto)
        {
            try
            {
                var imagensProduto = await _produto.ObterImagensProduto(codigoProduto);
                return Ok(imagensProduto);
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

        [HttpPost("CadastrarProduto")]
        public async Task<IActionResult> CadastrarProduto([FromBody] DTOProduto oDTOProduto)
        { 
            try
            {
                return Ok(await _produto.CadastrarProduto(oDTOProduto));
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
        [HttpPost("AdicionarImagem")]
        public async Task<IActionResult> AdicionarImagem([FromForm] ProdutoImagemRequest oProdutoImagemRequest)
        {
            try
            {   
                return Ok(await _produto.AdicionarImagem(oProdutoImagemRequest.codigoProduto, oProdutoImagemRequest.Imagem, oProdutoImagemRequest.Descricao));
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
        [HttpPut("EditarVariacaoProduto")]
        public async Task<IActionResult> EditarVariacaoProduto([FromBody] EditarVariacaoProdutoRequest request)
        {
            try
            {
                return Ok(await _produto.EditarVariacaoProduto(request.Codigo, request.variacoes));
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

        [HttpPut("AtualizarProduto")]
        public async Task<IActionResult> AtualizarProduto([FromBody] CWProduto oCWProduto)
        {
            try
            {
                return Ok(await _produto.AtualizarProduto(oCWProduto));
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
        [HttpDelete("ExcluirImagem/{codigoImagem}")]
        public async Task<IActionResult> ExcluirImagem(int codigoImagem)
        {
            try
            {
                return Ok(await _produto.ExcluirImagem(codigoImagem));
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
        [HttpDelete("ExcluirProdutos")]
        public async Task<IActionResult> ExcluirProdutos([FromBody] string arrCodigoProdutos)
        {
            try
            {
                return Ok(await _produto.ExcluirProdutos(arrCodigoProdutos));
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
        #endregion
    }
}