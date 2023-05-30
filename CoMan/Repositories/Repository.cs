using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CoMan.Data;
using CoMan.Extensions;

namespace CoMan.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly CoManDbContext Context = null!;

        public Repository(CoManDbContext context)
        {
            this.Context = context;
        }

        public ValueTask<TEntity> GetByIdAsync(int id)
        {
            return Context.Set<TEntity>().FindAsync(id)!;
        }

        public ValueTask<TEntity> GetByIdAsync(string id)
        {
            return Context.Set<TEntity>().FindAsync(id)!;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<dynamic> FindForDatatables(Expression<Func<TEntity, bool>> predicate,
            int start, int length, string member, bool ascending)
        {

            var totalCount = await Context.Set<TEntity>().CountAsync();

            var filtered = Context.Set<TEntity>().Where(predicate);
            var filteredCount = await filtered.CountAsync();

            var results = await filtered.OrderByDynamic<TEntity>(member, ascending).Skip(start).Take(length).ToListAsync();

            return new
            {
                Results = results,
                TotalCount = totalCount,
                FilteredCount = filteredCount
            };
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefaultAsync(predicate)!;
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }


        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
    }
}