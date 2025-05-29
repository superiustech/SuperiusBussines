using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Requests;
using Domain.ViewModel;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IUsuario
    {
        Task<List<DTOUsuario>> PesquisarUsuarios();
        Task<List<DTOPerfil>> PesquisarPerfisAtivos();
        Task<List<DTOFuncionalidade>> FuncionalidadesUsuario(string sCdUsuario);
        Task<DTORetorno> CadastrarUsuario(DTOUsuario oDTOUsuario);
        Task<DTORetorno> EditarUsuario(DTOUsuario oDTOUsuario);
        Task<DTORetorno> AssociarDesassociarPerfis(AssociacaoUsuarioRequest associacaoRequest);
        Task<DTORetorno> AssociarPerfis(AssociacaoUsuarioRequest associacaoRequest);
        Task<DTORetorno> DesassociarPerfis(AssociacaoUsuarioRequest associacaoRequest);
        Task<List<DTOPerfil>> PerfisAssociados(string codigoUsuario);
        bool ValidarFuncionalidades(params enumFuncionalidades[] funcionalidades);
        string RetornarCodigoUsuario();
    }
}
