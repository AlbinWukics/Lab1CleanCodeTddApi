using Shared.DataTransferObjects;

namespace Server.Endpoints.Cart.Get.GetAllCarts;

public class GetAllCartsResponse
{
    public IEnumerable<CartDto> Carts { get; set; } = null!;
}