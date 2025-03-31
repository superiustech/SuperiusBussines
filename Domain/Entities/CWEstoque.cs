using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CWEstoque
    {
        [Key]
        public int nCdEstoque { get; set; }
        public string sNmEstoque{ get; set; }
        public string sDsEstoque { get; set; }
        public string sCdEstoque { get; set; }
        public string sDsRua { get; set; }
        public string sDsComplemento { get; set; }
        public string sNrNumero { get; set; }
        public string sCdCep { get; set; }
    }
}
