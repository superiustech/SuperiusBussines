using Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IEstoqueRepository
    {
        Task<List<CWEstoque>> PesquisarTodos(int page = 0, int pageSize = 0, CWEstoque? oCWEstoqueFiltro = null);
        Task<List<CWEstoque>> PesquisarSemRevendedores(int? nCdRevendedor = null);
        Task<List<CWEstoque>> PesquisarTodosEstoques();
        Task<int> PesquisarQuantidadePaginas(CWEstoque? cwEstoqueFiltro = null);
        Task<int> CadastrarEstoque(CWEstoque oCWEstoque, List<CWEstoqueProduto> lstEstoqueProduto);
        Task<CWEstoque> Consultar(int nCdEstoque);
        Task<List<CWEstoqueProduto>> PesquisarPorEstoqueProduto(int nCdEstoque);
        Task<bool> AdicionarEstoqueProduto(CWEstoqueProduto oCWEstoqueProduto);
        Task AdicionarEditarProdutoEstoque(CWEstoqueProduto oCWEstoqueProduto);
        Task RemoverEstoqueProduto(int nCdEstoque, int nCdProduto);
        Task ExcluirEstoques(List<CWEstoque> lstEstoques);
    }
}
