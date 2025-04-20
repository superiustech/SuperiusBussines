using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Business.Services
{
    public class RevendedorService : IRevendedor
    {
        private readonly IRevendedorRepository _revendedorRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RevendedorService(IRevendedorRepository revendedorRepository, IHttpContextAccessor httpContextAccessor)
        {
            _revendedorRepository = revendedorRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CWRevendedor>> PesquisarRevendedores(int page, int pageSize, CWRevendedor? oCWRevendedorFiltro = null)
        {
            try
            {
                return await _revendedorRepository.PesquisarRevendedores(page, pageSize, oCWRevendedorFiltro);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao pesquisar revendedores.", ex);

            }
        }
        public async Task<int> PesquisarQuantidadePaginas(CWRevendedor? oCWRevendedorFiltro = null)
        {
            return await _revendedorRepository.PesquisarQuantidadePaginas(oCWRevendedorFiltro);
        }
        public async Task<int> CadastrarRevendedor(CWRevendedor oCWRevendedor)
        {
            try
            {
                return await _revendedorRepository.CadastrarRevendedor(oCWRevendedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao cadastrar o revendedor.", ex);
            }
        }
        public async Task<CWRevendedor> Consultar(int nCdRevendedor)
        {
            try
            {
                if (nCdRevendedor > 0)
                {
                    return await _revendedorRepository.Consultar(nCdRevendedor);
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao consultar o revendedor. Código inválido.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao consultar o revendedor.", ex);

            }
        }
        public async Task<List<CWRevendedorTipo>> PesquisarTipos()
        {
            try
            {
                return await _revendedorRepository.PesquisarTipos();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao pesquisar os tipos do revendedor.", ex);
            }
        }
    }
}
