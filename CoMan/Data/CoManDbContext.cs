using CoMan.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoMan.Data
{
    public class CoManDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TeacherUser> TeacherUsers { get; set; } = null!;
        public DbSet<TopicModel> Topics { get; set; } = null!;
        public DbSet<CooperationRequestModel> CooperationRequests { get; set; } = null!;
        public DbSet<CooperationModel> Cooperations { get; set; } = null!;
        public DbSet<CommentModel> Comments { get; set; } = null!;

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