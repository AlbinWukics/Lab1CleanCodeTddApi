using FastEndpoints;

namespace Server.Endpoints.Order;

public class OrderApiGroup : SubGroup<ApiGroup>
{
    public OrderApiGroup()
    {
        Configure("/order", ep =>
        {
            ep.Description(x => x
                .Produces(401)
                .WithTags("Order"));
        });
    }
}