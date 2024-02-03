using Shared.DataTransferObjects;

namespace Server.Endpoints.Order.Post;

public class AddOrderResponse
{
    public OrderDto? OrderDto { get; set; }
}