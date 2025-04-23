using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Requests
{
    public class EditarVariacaoProdutoRequest
    {
        public int nCdProduto { get; set; }
        public List<CWVariacao> variacoes { get; set; }
    }
    public class ProdutoImagemRequest
    {
        public int nCdProduto { get; set; }
        public IFormFile Imagem { get; set; }
        public string Descricao { get; set; }
    }
    public class FiltroRequest
    {
        public string? sNmFiltro { get; set; }
        public string? sDsFiltro { get; set; }
    }

     public class PaginacaoRequest {
         public int page { get; set; } = 1;
         public int pageSize { get; set; } = 10;
         public FiltroRequest? oFiltroRequest { get; set; }
     }
    public class EstoqueProdutoViewModel
    {
        public CWEstoque Estoque { get; set; }
        public List<ProdutoEstoqueDTO> Produtos { get; set; }
        public List<CWProduto> TodosProdutos { get; set; }

    }
    public class ProdutoEstoqueDTO
    {
        public CWProduto Produto { get; set; }
        public CWEstoqueProduto EstoqueProduto { get; set; }
    }
    public class EstoqueProdutoRequest
    {
        public int nCdEstoque { get; set; }
        public int nCdProduto { get; set; }
        public int dQtMinima { get; set; }
        public int dQtEstoque { get; set; }
        public decimal dVlVenda { get; set; }
        public decimal dVlCusto { get; set; }
    }
    public class EstoqueRequest
    {
        public int nCdEstoque { get; set; }
        public int nCdProduto { get; set; }
    }
    public class ProdutoEstoqueResultDTO
    {
        public EstoqueDTO Estoque { get; set; }
        public List<ProdutoEstoqueDTO> Produtos { get; set; }
        public List<ProdutoDTO> TodosProdutos { get; set; }
    }

    public class EstoqueDTO
    {
        public int nCdEstoque { get; set; }
        // Outras propriedades necessárias do estoque
    }

    public class ProdutoDTO
    {
        public int nCdProduto { get; set; }
        // Outras propriedades necessárias do produto
    }

    public class EstoqueProdutoDTO
    {
        public int nCdEstoque { get; set; }
        public int nCdProduto { get; set; }
        public int dQtMinima { get; set; }
        public int dQtEstoque { get; set; }
        public string sNmProduto { get; set; }
        public string sDsProduto { get; set; }
        public decimal dVlVenda { get; set; }
        public decimal dVlCusto { get; set; }
    }
}
