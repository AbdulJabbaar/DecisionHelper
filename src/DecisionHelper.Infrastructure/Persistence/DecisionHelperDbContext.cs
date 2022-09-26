using DecisionHelper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DecisionHelper.Infrastructure.Persistence
{
    public class DecisionHelperDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Adventure> Adventures { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<UserChoice> UserChoices { get; set; } = null!;

        public DecisionHelperDbContext(DbContextOptions<DecisionHelperDbContext> options):base(options)
        {
            
        }
    }
}
