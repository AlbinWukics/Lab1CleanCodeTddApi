using FastEndpoints;

namespace Server.Endpoints.Cart.Get.GetCartById;

public class GetCartByIdRequest
{
    [FromHeader]
    public string Id { get; set; } = string.Empty;
}