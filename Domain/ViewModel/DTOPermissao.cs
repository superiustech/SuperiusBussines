using Domain.Entities.Uteis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class DTOPermissao
    {
        [CampoObrigatorio]
        public int CodigoPermissao { get; set; }  

        [CampoObrigatorio]
        public string NomePermissao { get; set; }  

        public string DescricaoPermissao { get; set; } 

        [CampoObrigatorio]
        public bool Ativa { get; set; } 
    }
}