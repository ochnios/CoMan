using CoMan.Data;
using CoMan.Models;

namespace CoMan.Repositories
{
    public class StudentRepository : Repository<StudentUser>, IStudentRepository
    {
        public StudentRepository(CoManDbContext context)
            : base(context)
        { }

        private CoManDbContext CoManDbContext
        {
            get { return Context as CoManDbContext; }
        }
    }
}