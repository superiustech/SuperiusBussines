using Domain.Entities;
using Domain.Requests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IProduto
    {
        public Task<List<CWProduto>> PesquisarProdutos(int page, int pageSize, CWProduto? cwProdutoFiltro = null);
        public CWProduto ConsultarProduto(int nCdProduto);
        public Task<int> PesquisarQuantidadePaginas();
        public Task<List<CWVariacao>> ObterVariacoesAtivas();
        public Task<List<CWUnidadeMedida>> ObterUnidadesAtivas();
        public Task<List<CWProdutoImagem>> ObterImagensProduto(int nCdProduto);
        public Task CadastrarProduto(CWProduto cwProduto, List<CWVariacao> variacoes);
        public Task EditarVariacaoProduto(int nCdProduto, List<CWVariacao> variacoes);
        public Task AtualizarProduto(CWProduto cwProduto);
        public Task ExcluirImagem(int nCdImagem);
        public Task<List<dynamic>> ConsultarProdutoVariacao(int nCdProduto);
        public Task<CWProdutoImagem> AdicionarImagem(int nCdProduto, IFormFile Imagem, string Descricao);

    }
}