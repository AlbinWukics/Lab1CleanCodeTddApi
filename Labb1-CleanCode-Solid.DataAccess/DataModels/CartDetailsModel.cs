namespace Labb1_CleanCode_Solid.DataAccess.DataModels;

public class CartDetailsModel
{
    public Guid Id { get; set; }


    public int AmountOfProducts { get; set; }


    public CartModel Cart { get; set; } = null!;


    public ProductModel Product { get; set; } = null!;
}