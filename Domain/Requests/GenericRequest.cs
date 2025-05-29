using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Domain.ViewModel;

namespace Domain.Requests
{
    public class EditarVariacaoProdutoRequest
    {
        public int Codigo { get; set; }
        public List<DTOVariacao> variacoes { get; set; }
    }
    public class ProdutoImagemRequest
    {
        public int codigoProduto { get; set; }
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
        public int codigoEstoque { get; set; }
        public string arrCodigosProdutos { get; set; }
    }
    public class AssociacaoRequest
    {
        public int Codigo { get; set; }
        public string CodigosAssociacao { get; set; }
    }
    public class AssociacaoUsuarioRequest
    {
        public string Codigo { get; set; }
        public string CodigosAssociacao { get; set; }
    }
    public class AssociacaoRevendedorUsuarioRequest
    {
        public string CodigosRevendedores { get; set; }
        public string CodigosUsuarios { get; set; }
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
    public class EstoqueProdutoHistoricoDTO
    {
        public int Codigo { get; set; }
        public string EstoqueOrigem { get; set; }
        public string EstoqueDestino { get; set; }
        public string TipoMovimentacao { get; set; }
        public string DataMovimentacao { get; set; }
        public int QuantidadeMovimentada { get; set; }
        public string? Observacao { get; set; }
        public string? Produto { get; set; }
    }
    public class RevendedoresDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Estoque { get; set; }
    }
    public class ProdutoDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string CodigoSKU { get; set; }
        public string VideoURL { get; set; }
        public string Largura { get; set; }
        public string Comprimento { get; set; }
        public string Altura { get; set; }
        public string Peso { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal ValorUnitario { get; set; }
        public int UnidadeMedida { get; set; }
    }
    public class EstoqueDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string CodigoIdentificacao { get; set; }
        public string Rua { get; set; }
        public string Complemento { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
    }
}
