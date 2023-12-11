using API.Models;
using Microsoft.EntityFrameworkCore;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
        : base(options)
    {
    }

    public DbSet<OrderItem> OrderItems { get; set; } = null!;
}
