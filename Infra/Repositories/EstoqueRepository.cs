using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infra.Repositories
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly ApplicationDbContext _context;
        public EstoqueRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<CWEstoque> Consultar(int nCdEstoque)
        {
            return await _context.Estoque.AsNoTracking().FirstOrDefaultAsync(x => x.nCdEstoque == nCdEstoque);
        }
        public async Task<List<CWEstoque>> PesquisarTodos(int page = 0, int pageSize = 0, CWEstoque? oCWEstoqueFiltro = null)
        {
            if (page == 0 || pageSize == 0)
            {
                return await _context.Estoque.AsNoTracking().OrderBy(p => p.nCdEstoque).ToListAsync();
            }
            else
            {
                var query = _context.Estoque.AsNoTracking().AsQueryable();
                query = oCWEstoqueFiltro == null ? query : AplicarFiltros(query, oCWEstoqueFiltro);
                return await query.OrderBy(p => p.nCdEstoque).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }
        }
        private IQueryable<CWEstoque> AplicarFiltros(IQueryable<CWEstoque> query, CWEstoque filtro)
        {
            query = !string.IsNullOrEmpty(filtro.sNmEstoque) ? query.Where(p => EF.Functions.Like(p.sNmEstoque, $"%{filtro.sNmEstoque}%")) : query;
            query = !string.IsNullOrEmpty(filtro.sDsEstoque) ? query.Where(p => EF.Functions.Like(p.sDsEstoque, $"%{filtro.sDsEstoque}%")) : query;
            return query;
        }
        public async Task<int> PesquisarQuantidadePaginas(CWEstoque? cwEstoqueFiltro = null)
        {
            var query = _context.Estoque.AsNoTracking().AsQueryable();
            query = cwEstoqueFiltro == null ? query : AplicarFiltros(query, cwEstoqueFiltro);
            return await query.CountAsync();
        }
        public async Task<int> CadastrarEstoque(CWEstoque oCWEstoque, List<CWEstoqueProduto> lstEstoqueProduto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var estoqueExistente = await _context.Estoque.FirstOrDefaultAsync(p => p.nCdEstoque == oCWEstoque.nCdEstoque);
                int nCdEstoque = 0;
                if (estoqueExistente == null)
                {
                    await _context.Estoque.AddAsync(oCWEstoque);
                    await _context.SaveChangesAsync();
                    nCdEstoque = oCWEstoque.nCdEstoque;

                    List<CWEstoqueProduto> lstEstoqueProdutoInserir = new List<CWEstoqueProduto>();
                    foreach (var estoqueProduto in lstEstoqueProduto)
                    {
                        lstEstoqueProdutoInserir.Add(new CWEstoqueProduto
                        {
                            nCdEstoque = nCdEstoque,
                            nCdProduto = estoqueProduto.nCdProduto,
                        });
                    }

                    await _context.EstoqueProduto.AddRangeAsync(lstEstoqueProdutoInserir);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    estoqueExistente.sNmEstoque = oCWEstoque.sNmEstoque;
                    estoqueExistente.sDsEstoque = oCWEstoque.sDsEstoque;
                    estoqueExistente.sCdEstoque = oCWEstoque.sCdEstoque;
                    estoqueExistente.sDsRua = oCWEstoque.sDsRua;
                    estoqueExistente.sDsComplemento = oCWEstoque.sDsComplemento;
                    estoqueExistente.sCdCep = oCWEstoque.sCdCep;
                    estoqueExistente.sNrNumero = oCWEstoque.sNrNumero;

                    _context.Entry(estoqueExistente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    nCdEstoque = estoqueExistente.nCdEstoque;
                }
                await transaction.CommitAsync();
                return nCdEstoque;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<CWEstoqueProduto>> PesquisarPorEstoqueProduto(int nCdEstoque)
        {
            var produtosIds = await _context.EstoqueProduto.Where(ep => ep.nCdEstoque == nCdEstoque).Select(ep => ep.nCdProduto).Distinct().ToListAsync();
            return await _context.EstoqueProduto.Where(p => produtosIds.Contains(p.nCdProduto)).ToListAsync();
        }
        public async Task<bool> AdicionarEstoqueProduto(CWEstoqueProduto cwEstoqueProduto)
        {
            var estoqueExistente = await _context.EstoqueProduto.FirstOrDefaultAsync(ep => ep.nCdProduto == cwEstoqueProduto.nCdProduto && ep.nCdEstoque == cwEstoqueProduto.nCdEstoque);
            if (estoqueExistente == null)
            {
                _context.EstoqueProduto.Add(cwEstoqueProduto);
                await _context.SaveChangesAsync();
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task RemoverEstoqueProduto(int nCdEstoque, int nCdProduto)
        {
            _context.EstoqueProduto.Remove(new CWEstoqueProduto() { nCdEstoque = nCdEstoque, nCdProduto = nCdProduto });
            await _context.SaveChangesAsync();
        }
        public async Task AdicionarEditarProdutoEstoque(CWEstoqueProduto cwEstoqueProduto)
        {
            var estoqueExistente = await _context.EstoqueProduto.FirstOrDefaultAsync(ep => ep.nCdProduto == cwEstoqueProduto.nCdProduto && ep.nCdEstoque == cwEstoqueProduto.nCdEstoque);
            if (estoqueExistente != null)
            {
                estoqueExistente.dQtMinima = cwEstoqueProduto.dQtMinima;
                estoqueExistente.dQtEstoque = cwEstoqueProduto.dQtEstoque;
                estoqueExistente.dVlVenda = cwEstoqueProduto.dVlVenda;
                estoqueExistente.dVlCusto = cwEstoqueProduto.dVlCusto;
                _context.Entry(estoqueExistente).State = EntityState.Modified;
            }
            else
            {
                _context.EstoqueProduto.Add(cwEstoqueProduto);
            }

            await _context.SaveChangesAsync();
        }
    }
}
