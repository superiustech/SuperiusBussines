using Domain.Entities;
using Domain.Requests;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IPerfil
    {
        Task<List<DTOPerfil>> PesquisarPerfis();
        Task<DTORetorno> CadastrarPerfil(DTOPerfil oDTOPerfil);
        Task<DTORetorno> EditarPerfil(DTOPerfil oDTOPerfil);
        Task<DTORetorno> InativarPerfis(string arrCodigosPerfis);
        Task<DTORetorno> AtivarPerfis(string arrCodigosPerfis);
        Task<DTORetorno> AssociarPermissoes(AssociacaoRequest associacaoRequest);
        Task<DTORetorno> DesassociarPermissoes(AssociacaoRequest associacaoRequest);
        Task<List<DTOPermissao>> PermissoesAssociadas(int codigoPerfil);
    }
}