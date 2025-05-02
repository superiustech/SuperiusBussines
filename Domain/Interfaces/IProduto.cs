using Domain.Entities;
using Domain.Entities.ViewModel;
using Domain.Requests;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IProduto
    {
        public Task<List<DTOProduto>> PesquisarTodosProdutos();
        public Task<DTOProduto> ConsultarProduto(int nCdProduto);
        public Task<List<DTOVariacao>> ObterVariacoesAtivas();
        public Task<List<DTOVariacao>> ObterVariacoesAtivas(int? tipo);
        public Task<List<DTOUnidadeMedida>> ObterUnidadesAtivas();
        public Task<List<DTOProdutoImagem>> ObterImagensProduto(int nCdProduto);
        public Task<DTORetorno> CadastrarProduto(DTOProduto oDTOProduto);
        public Task<DTORetorno> EditarVariacaoProduto(int nCdProduto, List<DTOVariacao> variacoes);
        public Task<DTORetorno> AtualizarProduto(CWProduto cwProduto);
        public Task<DTORetorno> ExcluirImagem(int nCdImagem);
        public Task<List<DTOVariacao>> ConsultarProdutoVariacao(int nCdProduto);
        public Task<DTORetorno> AdicionarImagem(int nCdProduto, IFormFile Imagem, string Descricao);
        Task<DTORetorno> ExcluirProdutos(string arrCodigosProdutos);
    }
}