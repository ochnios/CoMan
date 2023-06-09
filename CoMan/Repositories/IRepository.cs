using CoMan.Models;
using System.Linq.Expressions;

namespace CoMan.Repositories
{
    public interface IRepository<TEntity> where TEntity : IDeletableEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByIdAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate);
        Task<dynamic> FindForDatatables(Expression<Func<TEntity, bool>> predicate, int start, int length, string member, bool ascending);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
    }
}