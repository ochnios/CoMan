﻿using CoMan.Repositories;

namespace CoMan.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoManDbContext _context;
        private TopicRepository _topicRepository;
        private CooperationRequestRepository _cooperationRequestRepository;

        public UnitOfWork(CoManDbContext context)
        {
            this._context = context;
        }

        public ITopicRepository Topics =>
            _topicRepository = _topicRepository ?? new TopicRepository(_context);

        public ICooperationRequestRepository CooperationRequests =>
            _cooperationRequestRepository = _cooperationRequestRepository ?? new CooperationRequestRepository(_context);

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