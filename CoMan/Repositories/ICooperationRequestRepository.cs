using CoMan.Models;
using CoMan.Repositories;
using System.Linq.Expressions;

namespace CoMan.Repositories
{
    public interface ICooperationRequestRepository : IRepository<CooperationRequestModel>
    {
        public Task<dynamic> FindForDatatables(Expression<Func<CooperationRequestModel, bool>> predicate,
                          int start, int length, string member, bool ascending, string currentUserId);
    }
}
