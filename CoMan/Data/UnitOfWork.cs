using CoMan.Repositories;

namespace CoMan.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoManDbContext _context = null!;
        private TeacherRepository _teacherRepository = null!;
        private StudentRepository _studentRepository = null!;
        private TopicRepository _topicRepository = null!;
        private CooperationRequestRepository _cooperationRequestRepository = null!;
        private CooperationRepository _cooperationRepository = null!;

        public UnitOfWork(CoManDbContext context)
        {
            this._context = context;
        }

        public ITeacherRepository Teachers =>
            _teacherRepository ??= new TeacherRepository(_context);

        public IStudentRepository Students =>
            _studentRepository ??= new StudentRepository(_context);

        public ITopicRepository Topics =>
            _topicRepository ??= new TopicRepository(_context);

        public ICooperationRequestRepository CooperationRequests =>
            _cooperationRequestRepository ??= new CooperationRequestRepository(_context);

        public ICooperationRepository Cooperations =>
            _cooperationRepository ??= new CooperationRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}