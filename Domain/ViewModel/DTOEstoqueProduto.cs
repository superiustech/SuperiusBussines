using Domain.Entities.Uteis;

namespace Domain.ViewModel
{
    public class DTOEstoqueProduto
    {
        [CampoObrigatorio]
        public int CodigoEstoque { get; set; }
        [CampoObrigatorio]
        public int CodigoProduto { get; set; }    
        public int QuantidadeMinima { get; set; }
        [CampoObrigatorio]
        public int QuantidadeEstoque { get; set; } 
        public string NomeProduto { get; set; }     
        public string DescricaoProduto { get; set; }
        public decimal ValorVenda { get; set; }     
        public decimal ValorCusto { get; set; }    
    }
}
