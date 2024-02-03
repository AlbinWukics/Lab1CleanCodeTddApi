using Shared.DataTransferObjects;

namespace Server.Endpoints.Order.Get.GetOrdersByCustomerId;

public class GetOrdersByCustomerIdResponse
{
    public IEnumerable<OrderDto> Orders { get; set; } = null!;
}