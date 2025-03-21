using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infra;
using System.Threading.Tasks;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UsuarioRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CWUsuario> ObterPorLogin(CWUsuario oCWUsuario)
    {
        if (string.IsNullOrEmpty(oCWUsuario.Email))
        {
            return null; 
        }

        var usuario = await _dbContext.Usuario
            .FirstOrDefaultAsync(u => u.Email == oCWUsuario.Email && u.Senha == oCWUsuario.Senha);

        return usuario;
    }
}
