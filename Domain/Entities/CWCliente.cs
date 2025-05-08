using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CWCliente
    {
        [Key]
        public int nCdCliente { get; set; }
        public string? sNmCliente { get; set; }
        public bool bFlAtivo { get; set; } = true;
        public string? CNPJ { get; set; }
        public ICollection<CWClienteUsuario>? Usuarios { get; set; }
    }
}