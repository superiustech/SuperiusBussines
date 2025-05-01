using Domain.Entities.Enum;
namespace Domain.Entities
{
    public class DTORetorno
    {
        public enumSituacao? Status { get; set; }
        public string Mensagem { get; set; }
        public object? Id { get; set; }
    }
}
