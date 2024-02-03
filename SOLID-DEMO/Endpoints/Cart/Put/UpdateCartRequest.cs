using FastEndpoints;
using Shared.DataTransferObjects;

namespace Server.Endpoints.Cart.Put;

public class UpdateCartRequest
{
    [FromBody]
    public CartDto CartDto { get; set; } = null!;
}