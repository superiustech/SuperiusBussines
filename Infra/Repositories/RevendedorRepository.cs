using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Azure;

namespace Infra.Repositories
{
    public class RevendedorRepository : IRevendedorRepository
    {
        private readonly ApplicationDbContext _context;
        public RevendedorRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<CWRevendedor> Consultar(int nCdRevendedor)
        {
            return await _context.Revendedor.AsNoTracking().FirstOrDefaultAsync(x => x.nCdRevendedor == nCdRevendedor);
        }
        public async Task ExcluirRevendedores(List<CWRevendedor> lstRevendedores)
        {
            _context.Revendedor.RemoveRange(lstRevendedores);
            await _context.SaveChangesAsync();
        }
        public async Task<List<CWRevendedor>> PesquisarRevendedoresSimples()
        {
            return await _context.Revendedor.AsNoTracking().OrderBy(p => p.nCdRevendedor).ToListAsync();
        }
        public async Task<List<CWRevendedorTipo>> PesquisarTipos()
        {
            return await _context.RevendedorTipo.AsNoTracking().Where(x => x.bFlAtivo == 1).OrderBy(x => x.nCdTipoRevendedor).ToListAsync();
        }
        public async Task<List<CWRevendedor>> PesquisarRevendedores()
        {
            return await _context.Revendedor.AsNoTracking().Include(t => t.Tipo).Include(e => e.Estoque).OrderBy(p => p.nCdRevendedor).ToListAsync();
        }
        public async Task CadastrarRevendedor(CWRevendedor oCWRevendedor)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var revendedorExistente = await _context.Revendedor.FirstOrDefaultAsync(p => p.nCdRevendedor == oCWRevendedor.nCdRevendedor);
                int nCdRevendedor = 0;

                if (revendedorExistente == null)
                {
                    if (oCWRevendedor.nCdEstoque == 0 || oCWRevendedor.nCdEstoque == null)
                    {
                        oCWRevendedor.nCdEstoque = null;
                        oCWRevendedor.Estoque = null;
                    }
                    else
                    {
                        oCWRevendedor.Estoque = await _context.Estoque.FindAsync(oCWRevendedor.nCdEstoque);
                    }

                    oCWRevendedor.Tipo = await _context.RevendedorTipo.FindAsync(oCWRevendedor.nCdTipoRevendedor);

                    await _context.Revendedor.AddAsync(oCWRevendedor);
                    await _context.SaveChangesAsync();
                    nCdRevendedor = oCWRevendedor.nCdRevendedor;
                }
                else
                {
                    revendedorExistente.sNmRevendedor = oCWRevendedor.sNmRevendedor;
                    revendedorExistente.sNrCpfCnpj = oCWRevendedor.sNrCpfCnpj;
                    revendedorExistente.dPcRevenda = oCWRevendedor.dPcRevenda;
                    revendedorExistente.sTelefone = oCWRevendedor.sTelefone;
                    revendedorExistente.sEmail = oCWRevendedor.sEmail;
                    revendedorExistente.sDsRua = oCWRevendedor.sDsRua;
                    revendedorExistente.sDsComplemento = oCWRevendedor.sDsComplemento;
                    revendedorExistente.sCdCep = oCWRevendedor.sCdCep;
                    revendedorExistente.sNrNumero = oCWRevendedor.sNrNumero;
                    revendedorExistente.nCdEstoque = (oCWRevendedor.nCdEstoque == 0) ? null : oCWRevendedor.nCdEstoque;
                    revendedorExistente.nCdTipoRevendedor = oCWRevendedor.nCdTipoRevendedor;

                    _context.Entry(revendedorExistente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    nCdRevendedor = revendedorExistente.nCdRevendedor;
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
