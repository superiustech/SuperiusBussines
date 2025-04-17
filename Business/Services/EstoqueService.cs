using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Business.Services
{
    public class EstoqueService : IEstoque
    {
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EstoqueService(IEstoqueRepository estoqueRepository, IHttpContextAccessor httpContextAccessor)
        {
            _estoqueRepository = estoqueRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CWEstoque>> PesquisarEstoques(int page, int pageSize, CWEstoque? oCWEstoquueFiltro = null)
        {
            return await _estoqueRepository.PesquisarTodos(page, pageSize, oCWEstoquueFiltro);
        }
        public async Task<int> PesquisarQuantidadePaginas(CWEstoque? cwEstoqueFIltro = null)
        {
            return await _estoqueRepository.PesquisarQuantidadePaginas(cwEstoqueFIltro);
        }
        public async Task<int> CadastrarEstoque(CWEstoque oCWEstoque, List<CWProduto> lstProduto)
        {
            try
            {
                List<CWEstoqueProduto> estoqueProduto = (lstProduto ?? new List<CWProduto>())
                .Where(p => p?.nCdProduto != null).Select(p => new CWEstoqueProduto
                 {
                     nCdProduto = p.nCdProduto, 
                     nCdEstoque = oCWEstoque?.nCdEstoque ?? 0 
                 }).Where(e => e.nCdEstoque != 0) .ToList();

                return await _estoqueRepository.CadastrarEstoque(oCWEstoque, estoqueProduto);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao cadastrar o estoque.", ex);
            }
        }
        public async Task<CWEstoque> Consultar(int nCdEstoque)
        {
            try
            {
                if (nCdEstoque > 0)
                {
                    return await _estoqueRepository.Consultar(nCdEstoque);
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao consultar o estoque. Código inválido.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao consultar o estoque.", ex);

            }
        }
        public async Task<List<CWEstoqueProduto>> PesquisarPorEstoqueProduto(int nCdEstoque)
        {
            try
            {
                if (nCdEstoque > 0)
                {
                    return await _estoqueRepository.PesquisarPorEstoqueProduto(nCdEstoque);
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao pesquisar produtos por estoque. Código inválido.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao pesquisar produtos por estoque.", ex);

            }
        }
        public async Task<bool> AdicionarEstoqueProduto(CWEstoqueProduto cwEstoqueProduto)
        {
            try
            {
               return await _estoqueRepository.AdicionarEstoqueProduto(cwEstoqueProduto);
            }
            catch(Exception ex)
            {
                throw new Exception("Ocorreu um erro ao consultar o estoque.", ex);
            }
        }
        public async Task RemoverEstoqueProduto(int nCdEstoque, int nCdProduto)
        {
            try
            {
                await _estoqueRepository.RemoverEstoqueProduto(nCdEstoque, nCdProduto);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao consultar o estoque.", ex);
            }
        }
        public async Task AdicionarEditarProdutoEstoque(CWEstoqueProduto cwEstoqueProduto)
        {
            try
            {
                await _estoqueRepository.AdicionarEditarProdutoEstoque(cwEstoqueProduto);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao consultar o estoque.", ex);
            }
        }

    }
    
}
