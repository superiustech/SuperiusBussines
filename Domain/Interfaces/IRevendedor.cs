using Domain.Entities;
using Domain.Requests;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IRevendedor
    {
        Task<DTORetorno> CadastrarRevendedor(DTORevendedor oDTORevendedor);
        Task<DTORevendedor> Consultar(int nCdRevendedor);
        Task<List<DTORevendedor>> PesquisarRevendedores();
        Task<List<DTOTipoRevendedor>> PesquisarTipos();
        Task<DTORetorno> ExcluirRevendedores(string arrCodigosRevendedores);
        Task<DTORetorno> AssociarDesassociarUsuarios(AssociacaoRevendedorUsuarioRequest associacaoRequest);
        Task<List<DTOUsuario>> UsuariosAssociados(int codigoRevendedor);
    }
}