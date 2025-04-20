using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CWRevendedor
    {
        [Key]
        public int nCdRevendedor { get; set; }
        public int nCdEstoque { get; set; }
        public int nCdTipoRevendedor { get; set; }
        public string sNmRevendedor { get; set; }
        public decimal dPcRevenda { get; set; }
        public string sNrCpfCnpj { get; set; }
        public string sTelefone { get; set; }
        public string sEmail { get; set; }
        public string sDsRua { get; set; }
        public string sDsComplemento { get; set; }
        public string sNrNumero { get; set; }
        public string sCdCep { get; set; }
    }

    public class CWRevendedorTipo
    {
        [Key]
        public int nCdTipoRevendedor { get; set; }
        public string sDsTipo { get; set; }
        public string sNmTipo { get; set; }
        public int bFlAtivo { get; set; }
    }
}
