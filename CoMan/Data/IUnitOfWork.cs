using CoMan.Repositories;

namespace CoMan.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ITopicRepository Topics { get; }
        ICooperationRequestRepository CooperationRequests { get; }
        Task<int> CommitAsync();
    }
}