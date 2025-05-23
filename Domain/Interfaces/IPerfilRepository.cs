using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPerfilRepository
    {
        Task<CWPerfil> CadastrarPerfil(CWPerfil cwPerfil);
        Task InativarPerfis(List<CWPerfil> lstPerfis);
        Task AtivarPerfis(List<CWPerfil> lstPerfis);
        Task AssociarPermissoes(int codigoPerfil, List<CWPermissao> lstPermissoes);
        Task DesassociarPermissoes(List<CWPermissaoPerfil> lstPermissoes);
        Task AssociarDesassociarPermissoes(int codigoPermissao, List<CWPermissao> lstPermissoes);
    }
}