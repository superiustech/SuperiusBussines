using Domain.Entities;
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
        public IActionResult ConsultarProduto(int nCdProduto)
        {
            CWProduto oCWProduto = _produto.ConsultarProduto(nCdProduto);
            return Ok(new { success = true, produto = oCWProduto});
        }

        [HttpGet("PesquisarProdutosComPaginacao")]
        public async Task<IActionResult> PesquisarProdutosComPaginacao([FromQuery] PaginacaoRequest oPaginacaoRequest)
        {
            try
            {
                CWProduto oCWProdutoFiltro = new CWProduto()
                {
                    sNmProduto = oPaginacaoRequest.oFiltroRequest?.sNmFiltro ?? string.Empty,
                    sDsProduto = oPaginacaoRequest.oFiltroRequest?.sDsFiltro ?? string.Empty
                };

                var produtos = await _produto.PesquisarProdutos(oPaginacaoRequest.page, oPaginacaoRequest.pageSize, oCWProdutoFiltro);
                var totalItens = await _produto.PesquisarQuantidadePaginas(oCWProdutoFiltro);
                var totalPaginas = (int)Math.Ceiling(totalItens / (double)oPaginacaoRequest.pageSize);

                return Ok(new { Produtos = produtos, TotalPaginas = totalPaginas, PaginaAtual = oPaginacaoRequest.page});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar produto");
                return StatusCode(500, $"Erro ao obter produtos: {ex.Message}");
            }
        }
        [HttpGet("Produtos")]
        public async Task<IActionResult> Produtos()
        {
            try
            {
                var produtos = await _produto.PesquisarTodosProdutos();
                List<ProdutoDTO> lstProdutosDTO = new List<ProdutoDTO>();

                lstProdutosDTO.AddRange(produtos.Select(x => new ProdutoDTO()
                {
                    Codigo = x.nCdProduto,
                    CodigoSKU = x.sCdProduto,
                    Nome = x.sNmProduto,
                    Descricao = x.sDsProduto,
                    ValorUnitario = x.dVlUnitario,
                    ValorVenda = x.dVlVenda
                }).ToList());

                return Ok(new { Produtos = lstProdutosDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar produto");
                return StatusCode(500, $"Erro ao obter produtos: {ex.Message}");
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
                var variacoes = await _produto.ObterVariacoesAtivas();
                return Ok(variacoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter unidade de medida: {ex.Message}");
            }
        }

        [HttpGet("OpcoesVariacao/{tipo}")]
        public async Task<IActionResult> GetOpcoesVariacao(int tipo)
        {
            try
            {
                var variacoes = await _produto.ObterVariacoesAtivas();
                return Ok(variacoes.Where(x => x.nCdVariacao == tipo));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter imagens: {ex.Message}");
            }
        }

        [HttpPost("CadastrarProduto")]
        public async Task<IActionResult> CadastrarProduto([FromBody] CWProduto dados)
        { 
            try
            {
                if (dados == null) return BadRequest("Dados inválidos.");

                int nCdProduto = await _produto.CadastrarProduto(dados);
                return Ok(new { success = true, message = "Dados salvos com sucesso.", codigoProduto = nCdProduto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar produto");
                return StatusCode(500, new { success = false, message = "Ocorreu um erro ao processar sua solicitação." + ex.Message });
            }
        }
        [HttpPost("AdicionarImagem")]
        public async Task<IActionResult> AdicionarImagem([FromForm] ProdutoImagemRequest oProdutoImagemRequest)
        {
            try
            {
                if (oProdutoImagemRequest.Imagem == null || oProdutoImagemRequest.Imagem.Length == 0)
                {
                    return BadRequest(new { success = false, message = "Nenhuma imagem foi enviada." });
                }

                CWProdutoImagem oCWProdutoImagem = await _produto.AdicionarImagem(
                    oProdutoImagemRequest.nCdProduto,
                    oProdutoImagemRequest.Imagem,
                    oProdutoImagemRequest.Descricao);

                return Ok(new { success = true, produtoImagem = oCWProdutoImagem });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpPut("EditarVariacaoProduto")]
        public async Task<IActionResult> EditarVariacaoProduto([FromBody] EditarVariacaoProdutoRequest request)
        {
            try
            {
                await _produto.EditarVariacaoProduto(request.nCdProduto, request.variacoes);
                return Ok(new { success = true, codigoProduto = request.nCdProduto, message = "Produto atualizado com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar variações: {ex.Message}");
            }
        }

        [HttpPut("AtualizarProduto")]
        public async Task<IActionResult> AtualizarProduto([FromBody] CWProduto oCWProduto)
        {
            try
            {
                await _produto.AtualizarProduto(oCWProduto);
                return Ok(new { success = true, message = "Produto atualizado com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpDelete("ExcluirImagem/{codigoImagem}")]
        public async Task<IActionResult> ExcluirImagem(int codigoImagem)
        {
            try
            {
                if (codigoImagem == 0) return BadRequest(new { success = false, message = "Código inválido." });

                await _produto.ExcluirImagem(codigoImagem);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpDelete("ExcluirProdutos")]
        public async Task<IActionResult> ExcluirProdutos([FromBody] string arrCodigoProdutos)
        {
            try
            {
                await _produto.ExcluirProdutos(arrCodigoProdutos);
                return Ok(new { success = true, message = "Produto(s) excluido(s) com sucesso." });
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