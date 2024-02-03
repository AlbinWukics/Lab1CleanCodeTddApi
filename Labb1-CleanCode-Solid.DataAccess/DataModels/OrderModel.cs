namespace Labb1_CleanCode_Solid.DataAccess.DataModels;

public class OrderModel
{
    public Guid Id { get; set; }


    public DateTime ShippingDate { get; set; }


    public DateTime CreatedDate { get; set; }


    public DateTime UpdatedDate { get; set; }


    public CustomerModel Customer { get; set; } = null!;


    public ICollection<OrderDetailsModel> OrderDetails { get; set; } = new List<OrderDetailsModel>();
}