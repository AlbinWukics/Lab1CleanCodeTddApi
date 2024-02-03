using FastEndpoints;

namespace Server.Endpoints.Product.Delete;

public class DeleteProductByIdRequest
{
    [FromHeader]
    public string Id { get; set; } = string.Empty;
}