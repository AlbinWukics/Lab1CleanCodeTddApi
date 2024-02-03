using Shared.DataTransferObjects;

namespace Server.Endpoints.Cart.Get.GetCartsByCustomerId;

public class GetCartsByCustomerIdResponse
{
    public IEnumerable<CartDto> Carts { get; set; } = null!;
}