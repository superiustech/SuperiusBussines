using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Infra;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class FuncionalidadeRepository : IFuncionalidadeRepository
{
    private readonly ApplicationDbContext _context;
    public FuncionalidadeRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<CWFuncionalidade> CadastrarFuncionalidade(CWFuncionalidade cwFuncionalidade)
    {
        var funcionalidadeExistente = await _context.Funcionalidade.FirstOrDefaultAsync(p => p.nCdFuncionalidade == cwFuncionalidade.nCdFuncionalidade);
        if (funcionalidadeExistente == null)
        {
            await _context.Funcionalidade.AddAsync(cwFuncionalidade);
            await _context.SaveChangesAsync();
            return cwFuncionalidade;
        }
        else
        {
            funcionalidadeExistente.sNmFuncionalidade = cwFuncionalidade.sNmFuncionalidade;
            funcionalidadeExistente.sDsFuncionalidade = cwFuncionalidade.sDsFuncionalidade;
            funcionalidadeExistente.bFlAtiva = cwFuncionalidade.bFlAtiva;

            _context.Entry(funcionalidadeExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return funcionalidadeExistente;
        }
    }
    public async Task InativarFuncionalidades(List<CWFuncionalidade> lstFuncionalidades)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var lstCodigosFuncionalidades = lstFuncionalidades.Select(p => p.nCdFuncionalidade).ToList();
                var funcionalidades = _context.Funcionalidade.Where(pov => lstCodigosFuncionalidades.Contains(Convert.ToInt32(pov.nCdFuncionalidade)));

                foreach(var funcionalidade in funcionalidades)
                {
                    funcionalidade.bFlAtiva = false;
                    _context.Entry(funcionalidade).State = EntityState.Modified;

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
    public async Task AtivarFuncionalidades(List<CWFuncionalidade> lstFuncionalidades)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var lstCodigosFuncionalidades = lstFuncionalidades.Select(p => p.nCdFuncionalidade).ToList();
                var funcionalidades = _context.Funcionalidade.Where(pov => lstCodigosFuncionalidades.Contains(Convert.ToInt32(pov.nCdFuncionalidade)));

                foreach (var funcionalidade in funcionalidades)
                {
                    funcionalidade.bFlAtiva = true;
                    _context.Entry(funcionalidade).State = EntityState.Modified;

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
    public async Task<List<CWFuncionalidade>> FuncionalidadesUsuario(string sCdUsuario)
    {
        var funcionalidades = await (
            from perfilUsuario in _context.PerfilUsuario
            where perfilUsuario.sCdUsuario == sCdUsuario
            join permissaoPerfil in _context.PermissaoPerfil on perfilUsuario.nCdPerfil equals permissaoPerfil.nCdPerfil
            join funcionalidadePermissao in _context.FuncionalidadePermissao on permissaoPerfil.nCdPermissao equals funcionalidadePermissao.nCdPermissao
            join funcionalidade in _context.Funcionalidade on funcionalidadePermissao.nCdFuncionalidade equals funcionalidade.nCdFuncionalidade
            where funcionalidade.bFlAtiva
            select funcionalidade
        )
        .Distinct()
        .ToListAsync();

        return funcionalidades;
    }



}
