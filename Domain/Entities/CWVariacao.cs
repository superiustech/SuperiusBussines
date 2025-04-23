using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CWVariacao
    {
        [Key]
        public int nCdVariacao { get; set; }
        public string sNmVariacao { get; set; }
        public string sDsVariacao { get; set; }
        public bool bFlAtiva { get; set; }
        public ICollection<CWVariacaoOpcao> VariacaoOpcoes { get; set; }
    }
}
