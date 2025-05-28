using FlowerShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using Nonexistent.Models;

namespace FlowerShopAPI.Data;

public class FlowerShopDbContext : DbContext
{
    public FlowerShopDbContext(DbContextOptions<FlowerShopDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId);
    }
}