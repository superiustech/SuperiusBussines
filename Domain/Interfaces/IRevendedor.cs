using Domain.Entities;
using Domain.Requests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IRevendedor
    {
        Task<int> CadastrarRevendedor(CWRevendedor cwRevendedor);
        Task<CWRevendedor> Consultar(int nCdRevendedor);
        Task<List<CWRevendedor>> PesquisarRevendedores(int page = 0, int pageSize = 0, CWRevendedor? cwRevendedorFiltro = null);
        Task<int> PesquisarQuantidadePaginas(CWRevendedor? cwRevendedorFiltro = null);
        Task<List<CWRevendedorTipo>> PesquisarTipos();
        Task ExcluirRevendedores(string arrCodigosRevendedores);
    }
}