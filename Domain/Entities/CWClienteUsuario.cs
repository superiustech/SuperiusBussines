using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CWClienteUsuario
    {
        [Key]
        public string sCdUsuario { get; set; }
        [Required]
        [MaxLength(255)]
        public string sSenha { get; set; } = string.Empty;
        public bool bFlAtivo { get; set; } = true;
        [ForeignKey("Cliente")]
        public int nCdCliente { get; set; }
        public CWCliente Cliente { get; set; } = null!;
    }
}
