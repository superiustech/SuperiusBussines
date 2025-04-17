using Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<CWProduto>> PesquisarTodos(int page = 0, int pageSize = 0, CWProduto? oCWProdutoFiltro = null);
        CWProduto ConsultarProduto(int nCdProduto);
        Task<int> PesquisarQuantidadePaginas(CWProduto? cwProdutoFiltro = null);
        Task<List<CWVariacao>> PesquisarVariacoes();
        Task<List<CWUnidadeMedida>> PesquisarUnidadeMedidas();
        Task<List<CWProdutoImagem>> PesquisarProdutoImagens(int nCdProduto);
        Task<int> CadastrarProduto(CWProduto cwProduto);
        Task EditarVariacaoProduto(int nCdProduto, List<CWProdutoOpcaoVariacao> variacoes);
        Task AtualizarProduto(CWProduto cwProduto);
        Task<List<CWVariacao>> ConsultarProdutoVariacao(int nCdProduto);
        Task AdicionarImagem(CWProdutoImagem oCWProdutoImagem);
        Task ExcluirImagem(int nCdImagem);
        Task<List<CWProduto>> PesquisarPorEstoque(int nCdEstoque);
    }
}
