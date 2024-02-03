using Shared.DataTransferObjects;

namespace Server.Endpoints.Customer.Put;

public class UpdateCustomerResponse
{
    public CustomerDto? CustomerDto { get; set; }
}