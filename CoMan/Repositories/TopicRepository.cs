using CoMan.Data;
using CoMan.Models;

namespace CoMan.Repositories
{
    public class TopicRepository : Repository<TopicModel>, ITopicRepository
    {
        public TopicRepository(CoManDbContext context)
            : base(context)
        { }

        private CoManDbContext CoManDbContext
        {
            get { return Context as CoManDbContext; }
        }
    }
}