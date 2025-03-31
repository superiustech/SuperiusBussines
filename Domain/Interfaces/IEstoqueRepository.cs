using Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IEstoqueRepository
    {
        Task<int> CadastrarEstoque(CWEstoque oCWEstoque, List<CWEstoqueProduto> lstEstoqueProduto);
        Task<CWEstoque> Consultar(int nCdEstoque);
        Task<List<CWEstoqueProduto>> PesquisarPorEstoqueProduto(int nCdEstoque);
        public Task<bool> AdicionarEstoqueProduto(CWEstoqueProduto oCWEstoqueProduto);
        public Task AdicionarEditarProdutoEstoque(CWEstoqueProduto oCWEstoqueProduto);
        public Task RemoverEstoqueProduto(int nCdEstoque, int nCdProduto);
    }
}
