using CoMan.Repositories;

namespace CoMan.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ITeacherRepository Teachers { get;  }
        IStudentRepository Students { get; }
        ITopicRepository Topics { get; }
        ICooperationRequestRepository CooperationRequests { get; }
        ICooperationRepository Cooperations { get; }
        ICommentRepository Comments { get; }
        Task<int> CommitAsync();
    }
}