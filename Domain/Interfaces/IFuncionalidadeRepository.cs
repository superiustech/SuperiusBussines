using Domain.Entities;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IFuncionalidadeRepository
    {
        Task<CWFuncionalidade> CadastrarFuncionalidade(CWFuncionalidade cwFuncionalidade);
        Task<List<CWFuncionalidade>> FuncionalidadesUsuario(string sCdUsuario);
        Task InativarFuncionalidades(List<CWFuncionalidade> lstFuncionalidades);
        Task AtivarFuncionalidades(List<CWFuncionalidade> lstFuncionalidades);
    }
}