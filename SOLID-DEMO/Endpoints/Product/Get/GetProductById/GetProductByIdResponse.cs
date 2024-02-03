using Shared.DataTransferObjects;

namespace Server.Endpoints.Product.Get.GetProductById;

public class GetProductByIdResponse
{
    public ProductDto ProductDto { get; set; } = null!;
}