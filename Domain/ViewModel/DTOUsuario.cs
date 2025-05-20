using Domain.Entities;
using Domain.Entities.Uteis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class DTOUsuario
    {
        [CampoObrigatorio]
        public string Usuario { get; set; }
        [CampoObrigatorio]
        public string NomeUsuario { get; set; }
        public string? Email { get; set; }
        [CampoObrigatorio]
        public string Senha { get; set; }

        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public List<DTOPerfil>? Perfis { get; set; } = new List<DTOPerfil>();
    }
}