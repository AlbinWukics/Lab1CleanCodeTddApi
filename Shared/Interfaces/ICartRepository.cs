using Shared.DataTransferObjects;

namespace Shared.Interfaces;

public interface ICartRepository : IRepository<CartDto>
{
    Task<ServiceResponse<IEnumerable<CartDto>>> GetByCustomerIdAsync(Guid customerId);
}