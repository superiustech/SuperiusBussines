using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CWEstoqueProduto
    {
        [Key, Column(Order = 0)]
        public int nCdEstoque { get; set; }

        [Key, Column(Order = 1)]
        public int nCdProduto { get; set; }
        public int dQtMinima { get; set; }
        public int dQtEstoque { get; set; }
        public decimal dVlVenda { get; set; }
        public decimal dVlCusto { get; set; }
        public bool bFlAtivo { get; set; } = true;

        [ForeignKey("nCdEstoque")]
        public virtual CWEstoque? Estoque { get; set; }

        [ForeignKey("nCdProduto")]
        public virtual CWProduto? Produto { get; set; }
        public virtual ICollection<CWEstoqueProdutoHistorico> Historicos { get; set; } = new List<CWEstoqueProdutoHistorico>();
    }
}
