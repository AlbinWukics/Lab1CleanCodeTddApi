using Shared.DataTransferObjects;

namespace Shared.Interfaces;

public interface IOrderRepository : IRepository<OrderDto>
{
    Task<ServiceResponse<IEnumerable<OrderDto>>> GetByCustomerIdAsync(Guid customerId);
}