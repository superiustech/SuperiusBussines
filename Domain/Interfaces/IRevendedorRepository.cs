using Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IRevendedorRepository
    {
        Task<int> CadastrarRevendedor(CWRevendedor oCWRevendedor);
        Task<CWRevendedor> Consultar(int nCdRevendedor);
        Task<List<CWRevendedor>> PesquisarRevendedores(int page = 0, int pageSize = 0, CWRevendedor? cwRevendedorFiltro = null);
        Task<List<CWRevendedor>> PesquisarRevendedoresSimples();
        Task<int> PesquisarQuantidadePaginas(CWRevendedor? cwRevendedorFiltro = null);
        Task<List<CWRevendedorTipo>> PesquisarTipos();
        Task ExcluirRevendedores(List<CWRevendedor> lstRevendedores);
    }
}
