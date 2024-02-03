using Shared.DataTransferObjects;

namespace Server.Endpoints.Cart.Post;

public class AddCartResponse
{
    public CartDto? CartDto { get; set; }
}