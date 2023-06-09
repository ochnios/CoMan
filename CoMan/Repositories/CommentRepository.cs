﻿using CoMan.Data;
using CoMan.Models;

namespace CoMan.Repositories
{
    public class CommentRepository : Repository<CommentModel>, ICommentRepository
    {
        public CommentRepository(CoManDbContext context)
            : base(context)
        { }

        private CoManDbContext CoManDbContext
        {
            get { return Context as CoManDbContext; }
        }
    }
}