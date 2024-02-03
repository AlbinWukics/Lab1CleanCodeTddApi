using FastEndpoints;

namespace Server.Endpoints.Order.Delete;

public class DeleteOrderByIdRequest
{
    [FromBody] public string Id { get; set; } = string.Empty;
}