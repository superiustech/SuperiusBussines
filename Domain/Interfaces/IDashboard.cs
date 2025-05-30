using Domain.ViewModel;
namespace Domain.Interfaces
{
    public interface IDashboard
    {
        Task<DTOResumoDashboard> ResumoDashboard();
        Task<List<DTOProdutoPorEstoque>> ProdutoPorEstoque();
    }
}
                                                                                                                                        