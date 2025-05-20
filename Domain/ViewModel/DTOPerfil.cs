using Domain.Entities.Uteis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class DTOPerfil
    {
        [CampoObrigatorio]
        public int CodigoPerfil { get; set; }  

        [CampoObrigatorio]
        public string NomePerfil { get; set; }  

        public string DescricaoPerfil { get; set; } 

        [CampoObrigatorio]
        public bool Ativa { get; set; } 
    }
}