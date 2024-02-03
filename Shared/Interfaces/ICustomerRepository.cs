using Shared.DataTransferObjects;

namespace Shared.Interfaces;

public interface ICustomerRepository : IRepository<CustomerDto>
{
    Task<ServiceResponse<CustomerDto>> LoginCustomerAsync(string username, string password);
}