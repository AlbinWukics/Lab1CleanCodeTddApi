namespace Shared.DataTransferObjects;

public class OrderDto
{
    public Guid Id { get; set; }


    public DateTime ShippingDate { get; set; }


    public DateTime CreatedDate { get; set; }


    public DateTime UpdatedDate { get; set; }


    public CustomerDto Customer { get; set; } = null!;


    public ICollection<OrderDetailsDto> OrderDetails { get; set; } = new List<OrderDetailsDto>();
}