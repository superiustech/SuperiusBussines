using Domain.Entities.Uteis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class DTOVariacaoOpcao
    {
        [CampoObrigatorio]
        public int Codigo { get; set; }  

        [CampoObrigatorio]
        public string Nome { get; set; }  

        public string Descricao { get; set; } 

        [CampoObrigatorio]
        public bool Ativa { get; set; } 

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Atrelado { get; set; } 
    }
}