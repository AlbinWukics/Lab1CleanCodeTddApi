using Shared.DataTransferObjects;

namespace Server.Endpoints.Customer.Post;

public class AddCustomerResponse
{
    public CustomerDto? CustomerDto { get; set; }
}