namespace Labb1_CleanCode_Solid.DataAccess.DataModels;

public class CartModel
{
    public Guid Id { get; set; }


    public DateTime CreatedDate { get; set; }


    public DateTime UpdatedDate { get; set; }


    public CustomerModel Customer { get; set; } = null!;


    public ICollection<CartDetailsModel> CartDetails { get; set; } = new List<CartDetailsModel>();
}