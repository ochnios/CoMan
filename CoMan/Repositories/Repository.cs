using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CoMan.Data;
using jQueryDatatableServerSideNetCore.Extensions;
using System;

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
            return Context.Set<TEntity>().FindAsync(id);
        }

        public ValueTask<TEntity> GetByIdAsync(string id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate,
            int start, int length, string member, bool ascending)
        {
            return await Context.Set<TEntity>()
                .Where(predicate)
                .OrderByDynamic<TEntity>(member, ascending)
                .Skip(start)
                .Take(length)
                .ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).CountAsync();
        }

        public async Task<int> CountAsync()
        {
            return await Context.Set<TEntity>().CountAsync();
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefaultAsync(predicate);
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