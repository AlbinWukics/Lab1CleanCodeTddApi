namespace Shared.DataTransferObjects;

public class CartDetailsDto
{
    public Guid Id { get; set; }


    public int AmountOfProducts { get; set; }


    public CartDto Cart { get; set; } = null!;


    public ProductDto Product { get; set; } = null!;
}