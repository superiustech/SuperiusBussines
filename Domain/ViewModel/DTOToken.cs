using Domain.Entities.Uteis;
namespace Domain.Entities.ViewModel
{
    public class DTOToken
    {
        [CampoObrigatorio]
        public string Login { get; set; }
        [CampoObrigatorio]
        public string Senha { get; set; }
    }
}
