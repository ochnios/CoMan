using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CoMan.Data;
using CoMan.Extensions;
using CoMan.Models;

namespace CoMan.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IDeletableEntity
    {
        protected readonly CoManDbContext Context = null!;

        public Repository(CoManDbContext context)
        {
            this.Context = context;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var foundEntity = await Context.Set<TEntity>().FindAsync(id);

            if (foundEntity != null && foundEntity.Deleted != true)
            {
                return foundEntity;
            }

            return null;
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var foundEntity = await Context.Set<TEntity>().FindAsync(id);

            if (foundEntity != null && foundEntity.Deleted != true)
            {
                return foundEntity;
            }

            return null;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await GetActiveEntities().Where(e => !e.Deleted).ToListAsync();
        }

        public async Task<dynamic> FindForDatatables(Expression<Func<TEntity, bool>> predicate,
            int start, int length, string member, bool ascending)
        {
            var activeEntities = GetActiveEntities();

            var totalCount = await activeEntities.CountAsync();
            var filtered = activeEntities.Where(predicate);
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
            return GetActiveEntities().SingleOrDefaultAsync(predicate)!;
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public void Remove(TEntity entity)
        {
            entity.Deleted = true;
        }

        private IQueryable<TEntity> GetActiveEntities()
        {
            return Context.Set<TEntity>().Where(e => !e.Deleted);
        }
    }
}