using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CWPermissao
    {
        [Key]
        public int nCdPermissao { get; set; }
        public string sNmPermissao { get; set; }
        public string sDsPermissao { get; set; }
        public bool bFlAtiva { get; set; }
    }
}
