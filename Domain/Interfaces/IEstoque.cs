using Domain.Entities;
using Domain.Requests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IEstoque
    {
        public Task<int> CadastrarEstoque(CWEstoque oCWEstoque, List<CWProduto> lstProdutos);
        public Task<CWEstoque> Consultar(int nCdEstoque);
        public Task<List<CWEstoqueProduto>> PesquisarPorEstoqueProduto(int nCdEstoque);
        public Task<bool> AdicionarEstoqueProduto(CWEstoqueProduto oCWEstoqueProduto);
        public Task AdicionarEditarProdutoEstoque(CWEstoqueProduto oCWEstoqueProduto);
        public Task RemoverEstoqueProduto(int nCdEstoque, int nCdProduto);

    }
}