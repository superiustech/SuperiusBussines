using Business.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Requests;
using DotNetNuke.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [HttpGet("PesquisarEstoquesComPaginacao")]
        public async Task<IActionResult> PesquisarEstoquesComPaginacao([FromQuery] PaginacaoRequest oPaginacaoRequest)
        {
            try
            {
                CWEstoque oCWEstoqueFiltro = new CWEstoque()
                {
                    sNmEstoque = oPaginacaoRequest.oFiltroRequest?.sNmFiltro ?? string.Empty,
                    sDsEstoque = oPaginacaoRequest.oFiltroRequest?.sDsFiltro ?? string.Empty
                };

                var estoques = await _estoque.PesquisarEstoques(oPaginacaoRequest.page, oPaginacaoRequest.pageSize, oCWEstoqueFiltro);
                var totalItens = await _estoque.PesquisarQuantidadePaginas(oCWEstoqueFiltro);
                var totalPaginas = (int)Math.Ceiling(totalItens / (double)oPaginacaoRequest.pageSize);

                return Ok(new
                {
                    Estoques = estoques,
                    TotalPaginas = totalPaginas,
                    PaginaAtual = oPaginacaoRequest.page
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter produtos: {ex.Message}");
            }
        }
        [HttpGet("Estoques")]
        public async Task<IActionResult> Produtos()
        {
            try
            {
                var estoques = await _estoque.PesquisarTodosEstoques();
                List<EstoqueDTO> lstEstoqueDTO = new List<EstoqueDTO>();

                lstEstoqueDTO.AddRange(estoques.Select(x => new EstoqueDTO()
                {
                    Codigo = x.nCdEstoque,
                    CodigoIdentificacao = x.sCdEstoque,
                    Nome = x.sNmEstoque,
                    Descricao = x.sDsEstoque,
                    Cep = x.sCdCep
                }).ToList());

                return Ok(new { Estoques = lstEstoqueDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pesquisar estoques");
                return StatusCode(500, $"Erro ao obter estoques: {ex.Message}");
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter produtos: {ex.Message}");
            }
        }
        [HttpGet("Estoque/{codigoEstoque}")]
        public async Task<IActionResult> Estoque(int codigoEstoque)
        {
            try
            {
                CWEstoque oCWEstoque = await _estoque.Consultar(codigoEstoque);
                oCWEstoque.Produtos = new List<CWEstoqueProduto>();
                return Ok(new { success = true, estoque = oCWEstoque });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter estoque: {ex.Message}");
            }
        }


        [HttpGet("EstoqueProduto/{codigoEstoque}")]
        public async Task<IActionResult> EstoqueProduto(int codigoEstoque)
        {
            try
            {
                CWEstoque estoque = await _estoque.Consultar(codigoEstoque);
                List<CWProduto> todosProdutos = await _produto.PesquisarProdutos();
                List<EstoqueProdutoDTO> lstEstoqueProdutoDTO = new List<EstoqueProdutoDTO>();

                lstEstoqueProdutoDTO.AddRange(estoque.Produtos.Select(x => new EstoqueProdutoDTO()
                {
                    nCdProduto = x.nCdProduto,
                    dQtMinima = x.dQtMinima,
                    dQtEstoque = x.dQtEstoque,
                    dVlVenda = x.dVlVenda,
                    dVlCusto = x.dVlCusto,
                    sNmProduto = x.Produto?.sNmProduto ?? string.Empty,
                    sDsProduto = x.Produto?.sDsProduto ?? string.Empty,
                }).ToList());

                return Ok(new { EstoqueProduto = lstEstoqueProdutoDTO, Produtos = todosProdutos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter estoque: {ex.Message}");
            }
        }
        [HttpPost("CadastrarEstoque")]
        public async Task<IActionResult> CadastrarEstoque([FromBody] CWEstoque estoque)
        {
            if (estoque == null) return BadRequest("Dados inválidos.");
            try
            {
                int nCdEstoque = await _estoque.CadastrarEstoque(estoque, new List<CWProduto>());
                return Ok(new { success = true, message = "Dados salvos com sucesso.", codigoEstoque = nCdEstoque });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar estoque");
                return StatusCode(500, $"Erro ao obter estoque: {ex.Message}");
            }
        }
        [HttpPost("AdicionarEstoqueProduto")]
        public async Task<IActionResult> AdicionarEstoqueProduto([FromBody] CWEstoqueProduto oCWEstoqueProduto)
        {
            try
            {
                if (oCWEstoqueProduto.nCdEstoque == 0 || oCWEstoqueProduto.nCdProduto == 0)
                {
                    return BadRequest(new { success = false, message = "Dados inválidos." });
                }

                var oEstoqueProduto = new CWEstoqueProduto()
                {
                    nCdEstoque = oCWEstoqueProduto.nCdEstoque,
                    nCdProduto = oCWEstoqueProduto.nCdProduto,
                    dQtEstoque = oCWEstoqueProduto?.dQtEstoque ?? 0,
                    dQtMinima = oCWEstoqueProduto?.dQtMinima ?? 0,
                    dVlCusto = oCWEstoqueProduto?.dVlCusto ?? 0,
                    dVlVenda = oCWEstoqueProduto?.dVlVenda ?? 0
                };

                bool bFLProdutoAtrelado = await _estoque.AdicionarEstoqueProduto(oEstoqueProduto);
                return Ok(new { success = true, produtoAtrelado = bFLProdutoAtrelado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpPut("EditarProdutoEstoque")]
        public async Task<IActionResult> EditarProdutoEstoque([FromBody] CWEstoqueProduto request)
        {
            try
            {
                if (request.nCdEstoque == 0 || request.nCdProduto == 0)
                    return BadRequest(new { success = false, message = "Dados inválidos." });

                await _estoque.AdicionarEditarProdutoEstoque(request);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpDelete("RemoverEstoqueProduto")]
        public async Task<IActionResult> RemoverEstoqueProduto([FromBody] EstoqueRequest request)
        {
            try
            {
                if (request.nCdEstoque == 0 || request.nCdProduto == 0)
                    return BadRequest(new { success = false, message = "Dados inválidos." });

                await _estoque.RemoverEstoqueProduto(request.nCdEstoque, request.nCdProduto);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpDelete("ExcluirEstoques")]
        public async Task<IActionResult> ExcluirEstoques([FromBody] string arrCodigoEstoques)
        {
            try
            {
                await _estoque.ExcluirEstoques(arrCodigoEstoques);
                return Ok(new { success = true, message = "Estoques(s) excluido(s) com sucesso." });
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