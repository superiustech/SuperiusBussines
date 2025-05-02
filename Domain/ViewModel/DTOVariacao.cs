using Domain.Entities.Uteis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class DTOVariacao
    {
        [CampoObrigatorio]
        public int Codigo { get; set; }  

        [CampoObrigatorio]
        public string Nome { get; set; } 

        public string Descricao { get; set; }  

        [CampoObrigatorio]
        public bool Ativa { get; set; } 

        public List<DTOVariacaoOpcao> Opcoes { get; set; } = new List<DTOVariacaoOpcao>(); 
    }
}