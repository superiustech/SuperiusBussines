using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CWUnidadeMedida
    {
        [Key]
        public int nCdUnidadeMedida {  get; set; }
        public string sCdUnidadeMedida { get; set; }
        public string sDsUnidadeMedida { get; set; }
        public string sSgUnidadeMedida { get; set; }
        public int bFlAtivo { get; set; }
    }
}
