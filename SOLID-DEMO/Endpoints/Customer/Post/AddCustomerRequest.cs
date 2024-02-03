using Shared.DataTransferObjects;

namespace Server.Endpoints.Customer.Post;

public class AddCustomerRequest
{
    public CustomerDto CustomerDto { get; set; } = null!;
}