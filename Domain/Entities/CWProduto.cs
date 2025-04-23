using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CWProduto
    {
        [Key]
        public int nCdProduto { get; set; }
        public string sNmProduto { get; set; }
        public string sDsProduto { get; set; }
        public string sCdProduto { get; set; }
        public string sUrlVideo { get; set; }
        public string sLargura { get; set; }
        public string sComprimento { get; set; }
        public string sAltura { get; set; }
        public string sPeso { get; set; }
        public decimal dVlVenda { get; set; }
        public decimal dVlUnitario { get; set; }
        public int nCdUnidadeMedida { get; set; }

        [ForeignKey("nCdUnidadeMedida")] 
        public CWUnidadeMedida? UnidadeMedida { get; set; }
        public ICollection<CWProdutoImagem> Imagens { get; set; } = new List<CWProdutoImagem>();
        public ICollection<CWProdutoOpcaoVariacao> OpcoesVariacoes { get; set; } = new List<CWProdutoOpcaoVariacao>();
    }
}
