using Shared.DataTransferObjects;

namespace Server.Endpoints.Product.Post;

public class AddProductRequest
{
    public ProductDto ProductDto { get; set; } = null!;
}