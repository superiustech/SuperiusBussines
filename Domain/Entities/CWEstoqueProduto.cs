using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class CWEstoqueProduto
    {
        [Key]
        public int nCdEstoque { get; set; }
        public int nCdProduto { get; set; }
        public int dQtMinima { get; set; }
        public int dQtEstoque { get; set; }
        public decimal dVlVenda { get; set; }
        public decimal dVlCusto { get; set; }
    
    }
}
