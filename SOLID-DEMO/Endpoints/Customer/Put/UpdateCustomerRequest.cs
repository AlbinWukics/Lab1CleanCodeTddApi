using Shared.DataTransferObjects;

namespace Server.Endpoints.Customer.Put;

public class UpdateCustomerRequest
{
    public CustomerDto CustomerDto { get; set; } = null!;
}