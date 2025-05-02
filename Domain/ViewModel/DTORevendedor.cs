using Domain.Entities.Uteis;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.ViewModel
{
    public class DTORevendedor
    {
        [CampoObrigatorio]
        public int Codigo { get; set; }
        public int? CodigoEstoque { get; set; }
        [NotMapped]
        public string? Estoque { get; set; }
        [CampoObrigatorio]
        public int CodigoTipoRevendedor { get; set; }
        [NotMapped]
        public string? TipoRevendedor { get; set; }
        [CampoObrigatorio]
        public string Nome { get; set; }
        public decimal PercentualRevenda { get; set; } = 0;
        [CampoObrigatorio]
        public string CpfCnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Rua { get; set; }
        public string Complemento { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
    }
}