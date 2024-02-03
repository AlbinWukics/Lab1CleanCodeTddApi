using Shared.DataTransferObjects;

namespace Server.Endpoints.Order.Get.GetOrderById;

public class GetOrderByIdResponse
{
    public OrderDto? OrderDto { get; set; }
}