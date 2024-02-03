using Shared.DataTransferObjects;

namespace Server.Endpoints.Cart.Post;

public class AddCartRequest
{
    public CartDto CartDto { get; set; } = null!;
}