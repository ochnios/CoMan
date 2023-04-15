using CoMan.Data;
using CoMan.Models;

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
    }
}