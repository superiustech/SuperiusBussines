using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Interfaces;

namespace Infra.Repositories
{
    public class EntidadeLeituraRepository : IEntidadeLeituraRepository
    {
        private readonly ApplicationDbContext _context;

        public EntidadeLeituraRepository(ApplicationDbContext context) {
            this._context = context;
        }
        public async Task<T> Consultar<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return await _context.Set<T>().AsNoTracking().Where(funcWhere).FirstOrDefaultAsync();
        }
 
        public async Task<List<T>> Pesquisar<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return await _context.Set<T>().AsNoTracking().Where(funcWhere).ToListAsync();
        }
        public async Task<List<T>> Pesquisar<T>(Expression<Func<T, T>> funcSelect, Expression<Func<T, bool>> funcWhere) where T : class
        {
            return await _context.Set<T>().AsQueryable().AsNoTracking().Where(funcWhere).Select(funcSelect).ToListAsync();
        }

        public async Task<bool> ValidarExistencia<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return await _context.Set<T>().AsNoTracking().AnyAsync(funcWhere);
        }

        public async Task<List<T>> PesquisarTodos<T>() where T : class
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }
    }
}
