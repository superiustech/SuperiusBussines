using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CWFuncionalidade
    {
        [Key]
        public int nCdFuncionalidade { get; set; }
        public string sNmFuncionalidade { get; set; }
        public string sDsFuncionalidade { get; set; }
        public bool bFlAtiva { get; set; }
    }
}
