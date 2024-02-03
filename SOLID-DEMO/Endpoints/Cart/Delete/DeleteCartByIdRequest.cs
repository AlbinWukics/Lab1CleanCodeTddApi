using FastEndpoints;

namespace Server.Endpoints.Cart.Delete;

public class DeleteCartByIdRequest
{
    [FromBody] public string Id { get; set; } = string.Empty;
}