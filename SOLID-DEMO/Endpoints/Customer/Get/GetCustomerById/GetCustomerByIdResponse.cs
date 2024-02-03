using Shared.DataTransferObjects;

namespace Server.Endpoints.Customer.Get.GetCustomerById;

public class GetCustomerByIdResponse
{
    public CustomerDto? CustomerDto { get; set; }
}