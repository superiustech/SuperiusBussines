using Domain.Entities;
using Domain.Requests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IProduto
    {
        public Task<List<CWProduto>> PesquisarProdutos(int page = 0, int pageSize = 0, CWProduto? cwProdutoFiltro = null);
        public Task<List<CWProduto>> PesquisarTodosProdutos();
        public CWProduto ConsultarProduto(int nCdProduto);
        public Task<int> PesquisarQuantidadePaginas(CWProduto? cwProdutoFiltro = null);
        public Task<List<CWVariacao>> ObterVariacoesAtivas();
        public Task<List<CWUnidadeMedida>> ObterUnidadesAtivas();
        public Task<List<CWProdutoImagem>> ObterImagensProduto(int nCdProduto);
        public Task<int> CadastrarProduto(CWProduto cwProduto);
        public Task EditarVariacaoProduto(int nCdProduto, List<CWVariacao> variacoes);
        public Task AtualizarProduto(CWProduto cwProduto);
        public Task ExcluirImagem(int nCdImagem);
        public Task<List<CWVariacao>> ConsultarProdutoVariacao(int nCdProduto);
        public Task<CWProdutoImagem> AdicionarImagem(int nCdProduto, IFormFile Imagem, string Descricao);
        Task<List<CWProduto>> PesquisarPorEstoque(int nCdEstoque);
        Task ExcluirProdutos(string arrCodigosProdutos);
    }
}