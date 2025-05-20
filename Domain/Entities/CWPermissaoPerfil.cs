using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CWPermissaoPerfil
    {
        [Key, Column(Order = 0)]
        public int nCdPermissao { get; set; }
        [Key, Column(Order = 1)]
        public int nCdPerfil { get; set; }

        [ForeignKey(nameof(nCdPermissao))]
        public CWPermissao Permissao { get; set; }

        [ForeignKey(nameof(nCdPerfil))]
        public CWPerfil Perfil { get; set; }


    }
}
