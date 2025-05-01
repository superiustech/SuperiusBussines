using Domain.Entities.Uteis;
namespace Domain.Entities.ViewModel
{
    public class DTOEstoque
    {
        [CampoObrigatorio]
        public int Codigo { get; set; }
        [CampoObrigatorio]
        public string Nome { get; set; }
        [CampoObrigatorio]
        public string Descricao { get; set; }
        [CampoObrigatorio]
        public string CodigoIdentificacao { get; set; }
        public string Rua { get; set; }
        public string Complemento { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
    }
}
