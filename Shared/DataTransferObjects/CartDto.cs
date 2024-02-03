namespace Shared.DataTransferObjects;

public class CartDto
{
    public Guid Id { get; set; }


    public DateTime CreatedDate { get; set; }


    public DateTime UpdatedDate { get; set; }


    public CustomerDto Customer { get; set; } = null!;


    public ICollection<CartDetailsDto> CartDetails { get; set; } = new List<CartDetailsDto>();
}