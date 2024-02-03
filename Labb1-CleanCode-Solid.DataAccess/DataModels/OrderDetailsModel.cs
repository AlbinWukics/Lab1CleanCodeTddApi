namespace Labb1_CleanCode_Solid.DataAccess.DataModels;

public class OrderDetailsModel
{
    public Guid Id { get; set; }


    public int AmountOfProducts { get; set; }


    public ProductModel Product { get; set; } = null!;


    public OrderModel Order { get; set; } = null!;
}