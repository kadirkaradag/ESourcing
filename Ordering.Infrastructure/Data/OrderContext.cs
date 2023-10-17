using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContext :DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                        .Property(o => o.UnitPrice)
                        .HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Order>()
                        .Property(o => o.TotalPrice)
                        .HasColumnType("decimal(18, 2)");
        }

    }
}
