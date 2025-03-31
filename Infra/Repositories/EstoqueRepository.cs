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
        public async Task<int> CadastrarEstoque(CWEstoque oCWEstoque, List<CWEstoqueProduto> lstEstoqueProduto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Estoque.AddAsync(oCWEstoque);
                await _context.SaveChangesAsync();
                int nCdEstoque = oCWEstoque.nCdEstoque;

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
            var produtoExistente = await _context.EstoqueProduto.FirstOrDefaultAsync(ep => ep.nCdProduto == cwEstoqueProduto.nCdProduto && ep.nCdEstoque == cwEstoqueProduto.nCdEstoque);
            if (produtoExistente == null)
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
            var produtoExistente = await _context.EstoqueProduto.FirstOrDefaultAsync(ep => ep.nCdProduto == cwEstoqueProduto.nCdProduto && ep.nCdEstoque == cwEstoqueProduto.nCdEstoque);
            if (produtoExistente != null)
            {
                produtoExistente.dQtMinima = cwEstoqueProduto.dQtMinima;
                produtoExistente.dQtEstoque = cwEstoqueProduto.dQtEstoque;
                produtoExistente.dVlVenda = cwEstoqueProduto.dVlVenda;
                produtoExistente.dVlCusto = cwEstoqueProduto.dVlCusto;
                _context.Entry(produtoExistente).State = EntityState.Modified;
            }
            else
            {
                _context.EstoqueProduto.Add(cwEstoqueProduto);
            }

            await _context.SaveChangesAsync();
        }
    }
}
