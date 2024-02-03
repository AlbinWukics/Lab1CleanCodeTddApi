using FastEndpoints;

namespace Server.Endpoints.Cart.Get.GetCartsByCustomerId;

public class GetCartsByCustomerIdRequest
{
    [FromBody] public string Id { get; set; } = null!;
}