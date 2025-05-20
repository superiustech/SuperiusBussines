using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CWClienteUsuario
    {
        [Key]
        public string sCdUsuario { get; set; }
        [ForeignKey("Cliente")]
        public int nCdCliente { get; set; }
        public CWCliente Cliente { get; set; } = null!;
    }
}
