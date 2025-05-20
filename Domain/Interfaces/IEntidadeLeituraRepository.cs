using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IEntidadeLeituraRepository
    {
        public Task<T> Consultar<T>(Expression<Func<T, bool>> funcWhere) where T : class;
        public Task<List<T>> Pesquisar<T>(Expression<Func<T, bool>> funcWhere) where T : class;
        public Task<List<T>> Pesquisar<T>(Expression<Func<T, T>> funcSelect, Expression<Func<T, bool>> funcWhere) where T : class;
        public Task<bool> ValidarExistencia<T>(Expression<Func<T, bool>> funcWhere) where T : class;
        public Task<List<T>> PesquisarTodos<T>() where T : class;
    }
}
