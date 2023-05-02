﻿using CoMan.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoMan.Data
{
    public class CoManDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TeacherUser> TeacherUsers { get; set; } = null!;
        public DbSet<TopicModel> Topics { get; set; } = null!;
        public DbSet<CooperationRequestModel> CooperationRequests { get; set; } = null!;
        public CoManDbContext(DbContextOptions<CoManDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}