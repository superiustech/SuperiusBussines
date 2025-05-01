using Domain.Entities.Uteis;

namespace Domain.Entities.ViewModel
{
    public class DTOProduto
    {
        [CampoObrigatorio]
        public int Codigo { get; set; }

        [CampoObrigatorio]
        public string Nome { get; set; }

        [CampoObrigatorio]
        public string Descricao { get; set; }

        [CampoObrigatorio]
        public string CodigoProduto { get; set; }

        public string UrlVideo { get; set; }
        public string Largura { get; set; }
        public string Comprimento { get; set; }
        public string Altura { get; set; }
        public string Peso { get; set; }

        [CampoObrigatorio]
        public decimal ValorVenda { get; set; }

        public decimal ValorUnitario { get; set; }

        [CampoObrigatorio]
        public int CodigoUnidadeMedida { get; set; }
    }
}