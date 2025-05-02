using Domain.Entities.Uteis;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class DTOTipoRevendedor
    {
        [CampoObrigatorio]
        public int Codigo { get; set; }  

        [CampoObrigatorio]
        public string Descricao { get; set; } 

        [CampoObrigatorio]
        public string Nome { get; set; }
        public bool Ativo { get; set; } = false;
    }
}