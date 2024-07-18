using Microsoft.EntityFrameworkCore;
using TarjetaCPruebaAPI.Models;

namespace TarjetaCPruebaAPI.Data
{
    public class CreditCardDbContext : DbContext
    {
        public CreditCardDbContext(DbContextOptions<CreditCardDbContext> options)
            : base(options)
        {
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<CreditCard>? CreditCards { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
