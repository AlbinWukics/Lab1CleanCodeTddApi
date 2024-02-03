using Shared.DataTransferObjects;

namespace Server.Endpoints.Order.Post;

public class AddOrderRequest
{
    public OrderDto OrderDto { get; set; } = null!;
}