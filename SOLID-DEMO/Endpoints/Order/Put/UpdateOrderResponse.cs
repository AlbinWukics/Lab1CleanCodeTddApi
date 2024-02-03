using Shared.DataTransferObjects;

namespace Server.Endpoints.Order.Put;

public class UpdateOrderResponse
{
    public OrderDto? OrderDto { get; set; }
}