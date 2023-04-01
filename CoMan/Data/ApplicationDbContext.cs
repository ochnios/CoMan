using CoMan.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoMan.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<TopicModel> Topics { get; set; } = null!;
        public DbSet<CooperationRequestModel> CooperationRequests { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}