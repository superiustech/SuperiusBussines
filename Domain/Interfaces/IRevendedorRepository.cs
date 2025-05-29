using Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IRevendedorRepository
    {
        Task CadastrarRevendedor(CWRevendedor oCWRevendedor);
        Task<CWRevendedor> Consultar(int nCdRevendedor);
        Task<List<CWRevendedor>> PesquisarRevendedores();
        Task<List<CWRevendedor>> PesquisarRevendedoresSimples();
        Task<List<CWRevendedorTipo>> PesquisarTipos();
        Task ExcluirRevendedores(List<CWRevendedor> lstRevendedores);
        Task AssociarDesassociarUsuarios(List<CWRevendedorUsuario> lstRevendedorUsuario);
    }
}
