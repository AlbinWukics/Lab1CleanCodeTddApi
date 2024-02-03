using Shared.DataTransferObjects;

namespace Server.Endpoints.Product.Delete;

public class DeleteProductByIdResponse
{
    public ProductDto ProductDto { get; set; } = null!;
}