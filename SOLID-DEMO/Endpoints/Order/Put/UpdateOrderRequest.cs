using Shared.DataTransferObjects;

namespace Server.Endpoints.Order.Put;

public class UpdateOrderRequest
{
    public OrderDto OrderDto { get; set; } = null!;
}