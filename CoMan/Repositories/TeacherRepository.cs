using CoMan.Data;
using CoMan.Models;

namespace CoMan.Repositories
{
    public class TeacherRepository : Repository<TeacherUser>, ITeacherRepository
    {
        public TeacherRepository(CoManDbContext context)
            : base(context)
        { }

        private CoManDbContext CoManDbContext
        {
            get { return Context as CoManDbContext; }
        }
    }
}