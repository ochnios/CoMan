using CoMan.Data;
using CoMan.Extensions;
using CoMan.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoMan.Repositories
{
    public class CooperationRequestRepository : Repository<CooperationRequestModel>, ICooperationRequestRepository
    {
        public CooperationRequestRepository(CoManDbContext context)
            : base(context)
        { }

        private CoManDbContext CoManDbContext
        {
            get { return Context as CoManDbContext; }
        }

        public async Task<dynamic> FindForDatatables(Expression<Func<CooperationRequestModel, bool>> predicate,
               int start, int length, string member, bool ascending, string currentUserId)
        {
            var activeCooperationRequests = GetActiveEntities().Where(e => e.Teacher!.Id == currentUserId || e.Student!.Id == currentUserId);

            var totalCount = await activeCooperationRequests.CountAsync();
            var filtered = activeCooperationRequests.Where(predicate);
            var filteredCount = await filtered.CountAsync();

            var results = await filtered.OrderByDynamic<CooperationRequestModel>(member, ascending).Skip(start).Take(length).ToListAsync();

            return new
            {
                Results = results,
                TotalCount = totalCount,
                FilteredCount = filteredCount
            };
        }
    }
}