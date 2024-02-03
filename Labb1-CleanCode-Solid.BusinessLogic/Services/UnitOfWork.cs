using Labb1_CleanCode_Solid.DataAccess.Contexts;
using Shared.Interfaces;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly ShopContext _context;

    public ICustomerRepository CustomerRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public ICartRepository CartRepository { get; }

    public UnitOfWork(ShopContext context)
    {
        _context = context;
        CustomerRepository = new CustomerRepository(_context);
        ProductRepository = new ProductRepository(_context);
        OrderRepository = new OrderRepository(_context);
        CartRepository = new CartRepository(_context);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}