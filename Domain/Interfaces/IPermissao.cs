using Domain.Entities;
using Domain.Requests;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IPermissao
    {
        Task<List<DTOPermissao>> PesquisarPermissoes();
        Task<DTORetorno> CadastrarPermissao(DTOPermissao oDTOPermissao);
        Task<DTORetorno> EditarPermissao(DTOPermissao oDTOPermissao);
        Task<DTORetorno> InativarPermissoes(string arrCodigosPermissoes);
        Task<DTORetorno> AtivarPermissoes(string arrCodigosPermissoes);
        Task<DTORetorno> AssociarFuncionalidades(AssociacaoRequest associacaoRequest);
        Task<DTORetorno> DesassociarFuncionalidades(AssociacaoRequest associacaoRequest);
        Task<List<DTOFuncionalidade>> FuncionalidadesAssociadas(int codigoPermissao);
    }
}