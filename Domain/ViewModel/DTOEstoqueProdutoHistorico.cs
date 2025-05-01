using Domain.Entities.Uteis;

namespace Domain.ViewModel
{
    public class DTOEstoqueProdutoHistorico
    {
        [CampoObrigatorio]
        public int Codigo { get; set; }
        [CampoObrigatorio]
        public int CodigoProduto { get; set; }
        [CampoObrigatorio]
        public int CodigoEstoqueOrigem { get; set; }
        [CampoObrigatorio]
        public int CodigoEstoqueDestino { get; set; }
        public string EstoqueOrigem { get; set; }
        public string EstoqueDestino { get; set; }
        [CampoObrigatorio]
        public string TipoMovimentacao { get; set; }
        public string DataMovimentacao { get; set; }
        [CampoObrigatorio]
        public int QuantidadeMovimentada { get; set; }
        public string Observacao { get; set; }
        public string? Produto { get; set; }
    }
}
