using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CWRevendedorUsuario
    {
        [Key, Column(Order = 0)]
        public int nCdRevendedor { get; set; }

        [Key, Column(Order = 1)]
        public string sCdUsuario { get; set; }

        [ForeignKey("nCdRevendedor")]
        public virtual CWRevendedor? Revendedor { get; set; }

        [ForeignKey("sCdUsuario")]
        public virtual CWUsuario? Usuario { get; set; }
    }
}
