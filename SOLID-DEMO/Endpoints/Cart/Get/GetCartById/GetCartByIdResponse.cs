using Shared.DataTransferObjects;

namespace Server.Endpoints.Cart.Get.GetCartById;

public class GetCartByIdResponse
{
    public CartDto CartDto { get; set; } = null!;
}