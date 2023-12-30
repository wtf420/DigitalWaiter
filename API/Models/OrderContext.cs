using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>()
            .HasMany(x => x.PurchasedItems)
            .WithOne()
            .HasForeignKey(x => x.OrderItemId)
            .HasForeignKey(x => x.FoodItemId);
    }

    public DbSet<OrderItem> OrderItems { get; set; } = null!;
}
