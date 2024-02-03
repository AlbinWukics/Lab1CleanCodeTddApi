using Shared.DataTransferObjects;

namespace Server.Endpoints.Product.Put;

public class UpdateProductResponse
{
    public ProductDto? ProductDto { get; set; }
}