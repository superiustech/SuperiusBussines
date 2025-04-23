using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CWUsuario
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}
