using CoMan.Data;
using CoMan.Models;

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
    }
}