using CoMan.Repositories;

namespace CoMan.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ITeacherRepository Teachers { get;  }
        IStudentRepository Students { get; }
        ITopicRepository Topics { get; }
        ICooperationRequestRepository CooperationRequests { get; }
        Task<int> CommitAsync();
    }
}