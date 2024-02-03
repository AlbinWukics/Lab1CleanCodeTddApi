namespace Shared.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository CustomerRepository { get; }
    IProductRepository ProductRepository { get; }
    IOrderRepository OrderRepository { get; }
    ICartRepository CartRepository { get; }

    Task SaveAsync();
}