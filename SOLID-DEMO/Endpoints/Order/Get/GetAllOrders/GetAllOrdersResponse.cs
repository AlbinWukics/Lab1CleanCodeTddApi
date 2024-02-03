using Shared.DataTransferObjects;

namespace Server.Endpoints.Order.Get.GetAllOrders;

public class GetAllOrdersResponse
{
    public IEnumerable<OrderDto> Orders { get; set; } = null!;
}