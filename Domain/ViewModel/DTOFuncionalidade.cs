using Domain.Entities.Uteis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class DTOFuncionalidade
    {
        [CampoObrigatorio]
        public int CodigoFuncionalidade { get; set; }  

        [CampoObrigatorio]
        public string NomeFuncionalidade { get; set; }  

        public string DescricaoFuncionalidade { get; set; } 

        [CampoObrigatorio]
        public bool Ativa { get; set; } 
    }
}