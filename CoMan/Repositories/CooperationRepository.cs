using CoMan.Data;
using CoMan.Extensions;
using CoMan.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoMan.Repositories
{
    public class CooperationRepository : Repository<CooperationModel>, ICooperationRepository
    {
        public CooperationRepository(CoManDbContext context)
            : base(context)
        { }

        private CoManDbContext CoManDbContext
        {
            get { return Context as CoManDbContext; }
        }

        public async Task<dynamic> FindForDatatables(Expression<Func<CooperationModel, bool>> predicate,
               int start, int length, string member, bool ascending, string currentUserId)
        {
            var activeCooperations = GetActiveEntities().Where(e => e.Teacher!.Id == currentUserId || e.Student!.Id == currentUserId);

            var totalCount = await activeCooperations.CountAsync();
            var filtered = activeCooperations.Where(predicate);
            var filteredCount = await filtered.CountAsync();

            var results = await filtered.OrderByDynamic<CooperationModel>(member, ascending).Skip(start).Take(length).ToListAsync();

            return new
            {
                Results = results,
                TotalCount = totalCount,
                FilteredCount = filteredCount
            };
        }
    }
}