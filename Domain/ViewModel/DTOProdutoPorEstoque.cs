using Domain.Entities.Uteis;
using Domain.Entities.ViewModel;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class DTOProdutoPorEstoque
    {
        public int CodigoProduto { get; set; }
        public string NomeProduto { get; set; }
        public List<DTOEstoqueDashboard> Estoques { get; set; } = new List<DTOEstoqueDashboard>();
    }
}