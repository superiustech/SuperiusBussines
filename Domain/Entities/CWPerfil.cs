using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CWPerfil
    {
        [Key]
        public int nCdPerfil { get; set; }
        public string sNmPerfil { get; set; }
        public string sDsPerfil{ get; set; }
        public bool bFlAtiva { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<CWPermissao>? Permissoes { get; set; } = null;
    }
}
