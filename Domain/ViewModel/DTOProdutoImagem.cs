using Domain.Entities.Uteis;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class DTOProdutoImagem
    {
        [CampoObrigatorio]
        public int CodigoImagem { get; set; }

        [CampoObrigatorio]
        public int CodigoProduto { get; set; }

        [StringLength(200)]
        public string Descricao { get; set; }

        [CampoObrigatorio]
        [StringLength(500)]
        public string Caminho { get; set; }
    }
}