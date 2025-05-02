using Domain.Entities.Uteis;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class DTOUnidadeMedida
    {
        [CampoObrigatorio]
        public int Codigo { get; set; }
        [CampoObrigatorio]
        public string CodigoIdentificacao { get; set; }
        [CampoObrigatorio]
        public string Descricao { get; set; }
        [CampoObrigatorio]
        public string Sigla { get; set; }
        [CampoObrigatorio]
        public bool Ativo { get; set; }
    }
}