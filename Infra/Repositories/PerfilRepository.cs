using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Infra;
using Microsoft.EntityFrameworkCore;
public class PerfilRepository : IPerfilRepository
{
    private readonly ApplicationDbContext _context;
    public PerfilRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<CWPerfil> CadastrarPerfil(CWPerfil cwPerfil)
    {
        var PerfilExistente = await _context.Perfil.FirstOrDefaultAsync(p => p.nCdPerfil == cwPerfil.nCdPerfil);
        if (PerfilExistente == null)
        {
            await _context.Perfil.AddAsync(cwPerfil);
            await _context.SaveChangesAsync();
            return cwPerfil;
        }
        else
        {
            PerfilExistente.sNmPerfil = cwPerfil.sNmPerfil;
            PerfilExistente.sDsPerfil = cwPerfil.sDsPerfil;
            PerfilExistente.bFlAtiva = cwPerfil.bFlAtiva;

            _context.Entry(PerfilExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return PerfilExistente;
        }
    }
    public async Task InativarPerfis(List<CWPerfil> lstPerfis)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var lstCodigosPerfis = lstPerfis.Select(p => p.nCdPerfil).ToList();
                var Perfis = _context.Perfil.Where(pov => lstCodigosPerfis.Contains(Convert.ToInt32(pov.nCdPerfil)));

                foreach (var Perfil in Perfis)
                {
                    Perfil.bFlAtiva = false;
                    _context.Entry(Perfil).State = EntityState.Modified;
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
    public async Task AtivarPerfis(List<CWPerfil> lstPerfis)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var lstCodigosPerfis = lstPerfis.Select(p => p.nCdPerfil).ToList();
                var Perfis = _context.Perfil.Where(pov => lstCodigosPerfis.Contains(Convert.ToInt32(pov.nCdPerfil)));

                foreach (var Perfil in Perfis)
                {
                    Perfil.bFlAtiva = true;
                    _context.Entry(Perfil).State = EntityState.Modified;

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
    public async Task AssociarPermissoes(int codigoPerfil, List<CWPermissao> lstPermissoes)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                List<CWPermissaoPerfil> lstPermissaoPerfil = new List<CWPermissaoPerfil>();
                var lstCodigosPermissoes = lstPermissoes.Select(p => p.nCdPermissao).ToList();
                var permissoes = _context.Permissao.Where(pov => lstCodigosPermissoes.Contains(Convert.ToInt32(pov.nCdPermissao)));

                foreach (var permissao in permissoes)
                {

                    lstPermissaoPerfil.Add(new CWPermissaoPerfil()
                    {
                        nCdPerfil = codigoPerfil, 
                        nCdPermissao = permissao.nCdPermissao
                    });
                }

                _context.PermissaoPerfil.AddRange(lstPermissaoPerfil);
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
    public async Task DesassociarPermissoes(List<CWPermissaoPerfil> lstPermissaoPerfil)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var chaves = lstPermissaoPerfil.Select(x => new { x.nCdPerfil, x.nCdPermissao }).ToList();
                var registros = await _context.PermissaoPerfil.Where(fp => chaves.Select(p => p.nCdPermissao).Contains(fp.nCdPermissao) && chaves.Select(f => f.nCdPerfil).Contains(fp.nCdPerfil)).ToListAsync();
                
                _context.PermissaoPerfil.RemoveRange(registros);
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
