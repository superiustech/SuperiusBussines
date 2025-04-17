using Domain.Entities;
using Domain.Requests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IEstoque
    {
        Task<int> CadastrarEstoque(CWEstoque oCWEstoque, List<CWProduto> lstProdutos);
        Task<int> PesquisarQuantidadePaginas(CWEstoque? cwEstoqueFiltro = null);
        Task<CWEstoque> Consultar(int nCdEstoque);
        Task<List<CWEstoqueProduto>> PesquisarPorEstoqueProduto(int nCdEstoque);
        Task<bool> AdicionarEstoqueProduto(CWEstoqueProduto oCWEstoqueProduto);
        Task AdicionarEditarProdutoEstoque(CWEstoqueProduto oCWEstoqueProduto);
        Task RemoverEstoqueProduto(int nCdEstoque, int nCdProduto);
        Task<List<CWEstoque>> PesquisarEstoques(int page = 0, int pageSize = 0, CWEstoque? cwEstoqueFiltro = null);
    }
}