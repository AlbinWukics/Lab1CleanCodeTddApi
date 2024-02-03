using FastEndpoints;

namespace Server.Endpoints.Customer.Get.GetCustomerById;

public class GetCustomerByIdRequest
{
    [FromHeader] public string Id { get; set; } = null!;
}