using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CWProdutoImagem
    {
        [Key]
        public int nCdImagem { get; set; }
        public int nCdProduto { get; set; }
        public string sDsImagem { get; set; }
        public string sDsCaminho { get; set; }
    }
}
