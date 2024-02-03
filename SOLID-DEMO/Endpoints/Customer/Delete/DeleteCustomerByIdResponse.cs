using Shared.DataTransferObjects;

namespace Server.Endpoints.Customer.Delete;

public class DeleteCustomerByIdResponse
{
    public CustomerDto? CustomerDto { get; set; }
}