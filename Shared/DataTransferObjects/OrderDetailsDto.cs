namespace Shared.DataTransferObjects;

public class OrderDetailsDto
{
    public Guid Id { get; set; }


    public int AmountOfProducts { get; set; }


    public ProductDto Product { get; set; } = null!;


    public OrderDto Order { get; set; } = null!;
}