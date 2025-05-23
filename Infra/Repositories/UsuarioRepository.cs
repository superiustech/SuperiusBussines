using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Infra;
using Microsoft.EntityFrameworkCore;
public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;
    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<CWUsuario> CadastrarUsuario(CWUsuario cwUsuario)
    {
        var usuarioExistente = await _context.Usuario.FirstOrDefaultAsync(p => p.sCdUsuario == cwUsuario.sCdUsuario);
        if (usuarioExistente == null)
        {
            await _context.Usuario.AddAsync(cwUsuario);
            await _context.SaveChangesAsync();
            return cwUsuario;
        }
        else
        {
            usuarioExistente.sCdUsuario = cwUsuario.sCdUsuario;
            usuarioExistente.sNmUsuario = cwUsuario.sNmUsuario;
            usuarioExistente.sSenha = cwUsuario.sSenha;
            usuarioExistente.sEmail = cwUsuario.sEmail;

            _context.Entry(usuarioExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return usuarioExistente;
        }
    }
    public async Task AssociarDesassociarPerfis(string codigoUsuario, List<CWPerfil> lstPerfis)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var associacoesExistentes = await _context.PerfilUsuario.Where(fp => fp.sCdUsuario == codigoUsuario).ToListAsync();

                _context.PerfilUsuario.RemoveRange(associacoesExistentes);
                await _context.SaveChangesAsync();
                if (lstPerfis != null && lstPerfis.Any())
                {
                    var codigosPerfis = lstPerfis.Select(f => f.nCdPerfil).ToList();
                    var PerfisValidas = await _context.Perfil.Where(f => codigosPerfis.Contains(f.nCdPerfil)).ToListAsync();

                    var novasAssociacoes = PerfisValidas
                    .Select(f => new CWPerfilUsuario
                    {
                        sCdUsuario = codigoUsuario,
                        nCdPerfil = f.nCdPerfil
                    }).ToList();

                    await _context.PerfilUsuario.AddRangeAsync(novasAssociacoes);
                    await _context.SaveChangesAsync();
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
    public async Task AssociarPerfis(string codigoUsuario, List<CWPerfil> lstPerfis)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                List<CWPerfilUsuario> lstPerfilUsuario = new List<CWPerfilUsuario>();
                var lstCodigosPerfis = lstPerfis.Select(p => p.nCdPerfil).ToList();
                var perfis = _context.Perfil.Where(pov => lstCodigosPerfis.Contains(Convert.ToInt32(pov.nCdPerfil)));

                foreach (var perfil in perfis)
                {

                    lstPerfilUsuario.Add(new CWPerfilUsuario()
                    {
                        sCdUsuario = codigoUsuario,
                        nCdPerfil = perfil.nCdPerfil
                    });
                }

                _context.PerfilUsuario.AddRange(lstPerfilUsuario);
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
    public async Task DesassociarPerfis(List<CWPerfilUsuario> lstPerfilUsuario)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var chaves = lstPerfilUsuario.Select(x => new { x.sCdUsuario, x.nCdPerfil }).ToList();
                var registros = await _context.PerfilUsuario.Where(fp => chaves.Select(p => p.nCdPerfil).Contains(fp.nCdPerfil) && chaves.Select(f => f.sCdUsuario).Contains(fp.sCdUsuario)).ToListAsync();

                _context.PerfilUsuario.RemoveRange(registros);
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
