namespace Labb1_CleanCode_Solid.DataAccess.DataModels;

public class ProductModel
{
    public Guid Id { get; set; }


    public string Name { get; set; } = string.Empty;


    public string Description { get; set; } = string.Empty;


    public decimal UnitPrice { get; set; }


    public DateTime CreatedDate { get; set; }


    public DateTime LastUpdatedDate { get; set; }
}