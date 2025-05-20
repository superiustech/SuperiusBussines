using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CWFuncionalidadePermissao
    {
        [Key, Column(Order = 0)]
        public int nCdFuncionalidade { get; set; }

        [Key, Column(Order = 1)]
        public int nCdPermissao { get; set; }  

        [ForeignKey(nameof(nCdFuncionalidade))]
        public CWFuncionalidade Funcionalidade { get; set; }

        [ForeignKey(nameof(nCdPermissao))]
        public CWPermissao Permissao { get; set; }
    }

}
