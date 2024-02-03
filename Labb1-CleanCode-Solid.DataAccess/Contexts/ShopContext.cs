using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Labb1_CleanCode_Solid.DataAccess.Contexts;

public class ShopContext : DbContext
{
    public DbSet<CustomerModel> Customer { get; set; }
    public DbSet<ProductModel> Product { get; set; }
    public DbSet<OrderModel> Order { get; set; }
    public DbSet<OrderDetailsModel> OrderDetail { get; set; }
    public DbSet<CartModel> Cart { get; set; }
    public DbSet<CartDetailsModel> CartDetail { get; set; }

    public ShopContext(DbContextOptions options) : base(options)
    {

    }
}