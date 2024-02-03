using Shared.DataTransferObjects;

namespace Server.Endpoints.Customer.Get.GetAllCustomers;

public class GetAllCustomersResponse
{
    public IEnumerable<CustomerDto>? Customers { get; set; }
}