using Shared.DataTransferObjects;

namespace Server.Endpoints.Product.Put;

public class UpdateProductRequest
{
    public ProductDto ProductDto { get; set; } = null!;
}