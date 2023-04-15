using CoMan.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoMan.Data
{
    public class CoManDbContext : IdentityDbContext
    {
        public DbSet<TopicModel> Topics { get; set; } = null!;
        public DbSet<CooperationRequestModel> CooperationRequests { get; set; } = null!;
        public CoManDbContext(DbContextOptions<CoManDbContext> options)
            : base(options)
        {
        }
    }
}