using Shared.DataTransferObjects;

namespace Server.Endpoints.Cart.Delete;

public class DeleteCartByIdResponse
{
    public CartDto CartDto { get; set; } = null!;
}