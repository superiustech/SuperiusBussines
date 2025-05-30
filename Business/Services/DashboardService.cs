using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.Entities.Uteis;
using Domain.Requests;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Business.Uteis;
using Domain.Entities.ViewModel;
using System.Runtime.InteropServices;
using Infra.Repositories;
namespace Business.Services
{
    public class DashboardService : IDashboard
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;
        private readonly IEstoque _estoque;
        private readonly IRevendedor _revendedor;
        private readonly IUsuario _usuario;
        private readonly IEstoqueRepository _estoqueRepository;
        public DashboardService(IDashboardRepository dashboardRepository, IEntidadeLeituraRepository entidadeLeituraRepository, IEstoqueRepository estoqueRepository, IEstoque estoque, IRevendedor revendedor, IUsuario usuario)
        {
            _dashboardRepository = dashboardRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
            _estoqueRepository = estoqueRepository;
            _estoque = estoque;
            _revendedor = revendedor;
            _usuario = usuario;
        }
        public async Task<DTOResumoDashboard> ResumoDashboard()
        {
            try
            {
                DTOResumoDashboard oDTOResumoDashboard = new DTOResumoDashboard();
                List<DTOEstoque> lstEstoques = await _estoque.PesquisarTodosEstoques();
                List<DTORevendedor> lstRevendedores = await _revendedor.PesquisarRevendedores();

                oDTOResumoDashboard.EstoquesAtivos = lstEstoques.Count.ToString();
                oDTOResumoDashboard.RevendedoresAtivos = lstRevendedores.Count.ToString();
                oDTOResumoDashboard.ProdutosCadastrados = lstEstoques.Sum(x => long.TryParse(x.ProdutosCadastrados, out var valor) ? valor : 0).ToString();
                oDTOResumoDashboard.MediaRevendedores = lstRevendedores.Select(x => x.PercentualRevenda).Average();

                return oDTOResumoDashboard;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOProdutoPorEstoque>> ProdutoPorEstoque()
        {
            List<CWEstoque> estoques;
            enumFuncionalidades[] funcionalidadesVisualizarTodosEstoques = { enumFuncionalidades.VisualizarTodosEstoques };

            if (_usuario.ValidarFuncionalidades(funcionalidadesVisualizarTodosEstoques))
            {
                estoques = await _estoqueRepository.PesquisarTodosEstoques();
            }
            else
            {
                string codigoUsuario = _usuario.RetornarCodigoUsuario();
                if (string.IsNullOrEmpty(codigoUsuario))
                {
                    return new List<DTOProdutoPorEstoque>();
                }
                estoques = await _estoqueRepository.PesquisarEstoquesDoUsuario(codigoUsuario);
            }

            var resultado = estoques
                .SelectMany(e => e.Produtos, (estoque, produtoEstoque) => new { estoque, produtoEstoque })
                .GroupBy(x => x.produtoEstoque.Produto.nCdProduto) 
                .Select(g => new DTOProdutoPorEstoque
                {
                    CodigoProduto = g.Key,
                    NomeProduto = g.First().produtoEstoque.Produto.sNmProduto,
                    Estoques = g.Select(x => new DTOEstoqueDashboard
                    {
                        Codigo = x.estoque.nCdEstoque,
                        Nome = x.estoque.sNmEstoque,
                        Quantidade = x.produtoEstoque.dQtEstoque
                    }).ToList()
                })
                .ToList();

            return resultado;
        }
    }
}
