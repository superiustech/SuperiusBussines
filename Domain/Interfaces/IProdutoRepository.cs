using Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<CWProduto>> PesquisarTodos(int page, int pageSize);
        CWProduto ConsultarProduto(int nCdProduto);
        Task<int> PesquisarQuantidadePaginas();
        Task<List<CWVariacao>> PesquisarVariacoes();
        Task<List<CWUnidadeMedida>> PesquisarUnidadeMedidas();
        Task<List<CWProdutoImagem>> PesquisarProdutoImagens(int nCdProduto);
        Task CadastrarProduto(CWProduto cwProduto, List<CWProdutoOpcaoVariacaoBase> variacoes);
        Task EditarVariacaoProduto(int nCdProduto, List<CWProdutoOpcaoVariacaoBase> variacoes);
        Task AtualizarProduto(CWProduto cwProduto);
        Task<List<dynamic>> ConsultarProdutoVariacao(int nCdProduto);
        Task AdicionarImagem(CWProdutoImagem oCWProdutoImagem);
        Task ExcluirImagem(int nCdImagem);

    }
}
