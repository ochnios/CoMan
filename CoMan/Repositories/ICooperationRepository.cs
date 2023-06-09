﻿using CoMan.Models;
using System.Linq.Expressions;

namespace CoMan.Repositories
{
    public interface ICooperationRepository : IRepository<CooperationModel>
    {
        public Task<dynamic> FindForDatatables(Expression<Func<CooperationModel, bool>> predicate,
                          int start, int length, string member, bool ascending, string currentUserId);
    }
}
