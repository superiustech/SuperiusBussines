using Domain.Entities.Uteis;
using System.Text.Json.Serialization;
namespace Domain.Entities.ViewModel
{
    public class DTOEstoqueDashboard
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string ProdutosCadastrados { get; set; }
        public decimal Quantidade { get; set; }
    }
}
