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
        Task<List<CWEstoque>> PesquisarEstoquesDoUsuario(string codigoUsuario);
        Task<int> PesquisarQuantidadePaginas(CWEstoque? cwEstoqueFiltro = null);
        Task CadastrarEstoque(CWEstoque oCWEstoque);
        Task<CWEstoque> Consultar(int nCdEstoque);
        Task<bool> AdicionarEstoqueProduto(CWEstoqueProduto oCWEstoqueProduto);
        Task AdicionarEditarProdutoEstoque(CWEstoqueProduto oCWEstoqueProduto);
        Task MovimentarEntradaSaida(CWEstoqueProduto oCWEstoqueProduto, CWEstoqueProdutoHistorico cwEstoqueProdutoHistorico);
        Task RemoverEstoqueProduto(List<CWEstoqueProduto> lstEstoqueProduto);
        Task ExcluirEstoques(List<CWEstoque> lstEstoques);
        Task<List<CWEstoqueProdutoHistorico>> ConsultarHistorico(int nCdEstoque);
        Task<decimal> ObterPercentualRevenda(int nCdEstoque);
        Task<CWEstoqueProduto> ObterEstoqueExistente(CWEstoqueProduto cwEstoqueProduto);
        void DefinirValorVenda(CWEstoqueProduto cwEstoqueProduto, decimal percentualRevenda);
        Task<CWEstoqueProduto> CarregarDadosProdutoEEstoque(CWEstoqueProduto cwEstoqueProduto);
        Task<List<CWEstoqueProdutoHistorico>> ConsultarHistoricoEstoques(List<int> codEstoques);
    }
}
