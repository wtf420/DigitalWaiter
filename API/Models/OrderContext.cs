using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
        : base(options)
    {
    }

    public DbSet<OrderItem> OrderItems { get; set; } = null!;
}
