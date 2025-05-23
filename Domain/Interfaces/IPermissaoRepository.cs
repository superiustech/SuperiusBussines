using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPermissaoRepository
    {
        Task<CWPermissao> CadastrarPermissao(CWPermissao cwPermissao);
        Task InativarPermissoes(List<CWPermissao> lstPermissoes);
        Task AtivarPermissoes(List<CWPermissao> lstPermissoes);
        Task AssociarFuncionalidades(int codigoPermissao, List<CWFuncionalidade> lstFuncionalidades);
        Task AssociarDesassociarFuncionalidades(int codigoPermissao, List<CWFuncionalidade> lstFuncionalidades);
        Task DesassociarFuncionalidades(List<CWFuncionalidadePermissao> lstFuncionalidades);
    }
}