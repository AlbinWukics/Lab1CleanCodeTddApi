using Shared.DataTransferObjects;

namespace Server.Endpoints.Product.Get.GetAllProducts;

public class GetAllProductsResponse
{
    public IEnumerable<ProductDto> Products { get; set; } = null!;
}