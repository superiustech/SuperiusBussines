using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CWVariacaoOpcao
    {
        [Key]
        public int nCdVariacaoOpcao { get; set; }
        public string sNmVariacaoOpcao { get; set; }
        public string sDsVariacaoOpcao { get; set; }
        public bool bFlAtiva { get; set; }
        [NotMapped]
        public bool bFlAtrelado { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<CWVariacao>? Variacoes { get; set; } = null;
    }
}
