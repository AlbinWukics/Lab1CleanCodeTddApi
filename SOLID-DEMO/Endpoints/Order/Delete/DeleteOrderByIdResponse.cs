using Shared.DataTransferObjects;

namespace Server.Endpoints.Order.Delete;

public class DeleteOrderByIdResponse
{
    public OrderDto OrderDto { get; set; } = null!;
}