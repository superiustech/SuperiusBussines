using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Infra;
using Microsoft.EntityFrameworkCore;
using System.Linq;
public class PermissaoRepository : IPermissaoRepository
{
    private readonly ApplicationDbContext _context;
    public PermissaoRepository(ApplicationDbContext context)
    {
        _context = context;
    }
     public async Task<CWPermissao> CadastrarPermissao(CWPermissao cwPermissao)
    {
        var permissaoExistente = await _context.Permissao.FirstOrDefaultAsync(p => p.nCdPermissao == cwPermissao.nCdPermissao);
        if (permissaoExistente == null)
        {
            await _context.Permissao.AddAsync(cwPermissao);
            await _context.SaveChangesAsync();
            return cwPermissao;
        }
        else
        {
            permissaoExistente.sNmPermissao = cwPermissao.sNmPermissao;
            permissaoExistente.sDsPermissao = cwPermissao.sDsPermissao;
            permissaoExistente.bFlAtiva = cwPermissao.bFlAtiva;

            _context.Entry(permissaoExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return permissaoExistente;
        }
    }
    public async Task InativarPermissoes(List<CWPermissao> lstPermissoes)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var lstCodigosPermissoes = lstPermissoes.Select(p => p.nCdPermissao).ToList();
                var permissoes = _context.Permissao.Where(pov => lstCodigosPermissoes.Contains(Convert.ToInt32(pov.nCdPermissao)));

                foreach(var permissao in permissoes)
                {
                    permissao.bFlAtiva = false;
                    _context.Entry(permissao).State = EntityState.Modified;

                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    public async Task AtivarPermissoes(List<CWPermissao> lstPermissoes)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var lstCodigosPermissoes = lstPermissoes.Select(p => p.nCdPermissao).ToList();
                var Permissoes = _context.Permissao.Where(pov => lstCodigosPermissoes.Contains(Convert.ToInt32(pov.nCdPermissao)));

                foreach (var permissao in Permissoes)
                {
                    permissao.bFlAtiva = true;
                    _context.Entry(permissao).State = EntityState.Modified;

                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    public async Task AssociarFuncionalidades(int codigoPermissao, List<CWFuncionalidade> lstFuncionalidades)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                List<CWFuncionalidadePermissao> lstFuncionalidadePermissao = new List<CWFuncionalidadePermissao>();
                var lstCodigosFuncionalidades = lstFuncionalidades.Select(p => p.nCdFuncionalidade).ToList();
                var funcionalidades = _context.Funcionalidade.Where(pov => lstCodigosFuncionalidades.Contains(Convert.ToInt32(pov.nCdFuncionalidade)));

                foreach (var funcionalidade in funcionalidades)
                {

                    lstFuncionalidadePermissao.Add(new CWFuncionalidadePermissao()
                    {
                        nCdPermissao = codigoPermissao, 
                        nCdFuncionalidade = funcionalidade.nCdFuncionalidade
                    });
                }

                _context.FuncionalidadePermissao.AddRange(lstFuncionalidadePermissao);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    public async Task DesassociarFuncionalidades(List<CWFuncionalidadePermissao> lstFuncionalidadePermissao)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var chaves = lstFuncionalidadePermissao.Select(x => new { x.nCdPermissao, x.nCdFuncionalidade }).ToList();
                var registros = await _context.FuncionalidadePermissao.Where(fp => chaves.Select(p => p.nCdPermissao).Contains(fp.nCdPermissao) && chaves.Select(f => f.nCdFuncionalidade).Contains(fp.nCdFuncionalidade)).ToListAsync();
                
                _context.FuncionalidadePermissao.RemoveRange(registros);
                await _context.SaveChangesAsync();
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
