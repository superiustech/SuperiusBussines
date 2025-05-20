using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CWUsuario
    {
        [Key, Column(Order=0)]
        public string sCdUsuario { get; set; }
        public string sNmUsuario { get; set; }
        public string? sEmail { get; set; }
        public string sSenha { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<CWPerfilUsuario>? Perfis { get; set; }
    }
}
