using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CWPerfilUsuario
    {
        [Key, Column(Order = 0)]
        public int nCdPerfil { get; set; }

        [Key, Column(Order = 1)]
        public string sCdUsuario { get; set; }

        [ForeignKey(nameof(nCdPerfil))]
        public CWPerfil Perfil { get; set; }

        [ForeignKey(nameof(sCdUsuario))]
        public CWUsuario Usuario { get; set; }
    }
}
