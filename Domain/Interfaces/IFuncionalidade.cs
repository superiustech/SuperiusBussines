using Domain.Entities;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IFuncionalidade
    {
        Task<List<DTOFuncionalidade>> PesquisarFuncionalidades();
        Task<DTORetorno> CadastrarFuncionalidade(DTOFuncionalidade oDTOFuncionalidade);
        Task<DTORetorno> EditarFuncionalidade(DTOFuncionalidade oDTOFuncionalidade);
        Task<DTORetorno> InativarFuncionalidades(string arrCodigosFuncionalidades);
        Task<DTORetorno> AtivarFuncionalidades(string arrCodigosFuncionalidades);
    }
}