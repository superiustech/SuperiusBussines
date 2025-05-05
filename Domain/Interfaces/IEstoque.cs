using Domain.Entities;
using Domain.Entities.ViewModel;
using Domain.Requests;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IEstoque
    {
        Task<DTORetorno> CadastrarEstoque(DTOEstoque oDTOEstoque);
        Task<DTOEstoque> Consultar(int nCdEstoque);
        Task<List<DTOEstoqueProduto>> PesquisarEstoqueProduto(int nCdEstoque);
        Task<DTORetorno> MovimentarEntradaSaida(DTOEstoqueProdutoHistorico oDTOEstoqueProdutoHistorico);
        Task<DTORetorno> AdicionarEditarProdutoEstoque(DTOEstoqueProduto oDTOEstoqueProduto);
        Task<DTORetorno> RemoverEstoqueProduto(int codigoEstoque, string arrCodigosProdutos);
        Task<List<DTOEstoque>> PesquisarEstoques(int? nCdRevendedor = null);
        Task<List<DTOEstoque>> PesquisarTodosEstoques();
        Task<DTORetorno> ExcluirEstoques(string arrCodigosEstoques);
        Task<List<DTOEstoqueProdutoHistorico>> ConsultarHistorico(int nCdEstoque);
        string ObterStringTipoMovimentacao(nTipoMovimentacao nTipo);
    }
}