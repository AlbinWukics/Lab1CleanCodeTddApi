using FastEndpoints;

namespace Server.Endpoints.Cart;

public sealed class CartApiGroup : SubGroup<ApiGroup>
{
    public CartApiGroup()
    {
        Configure("/cart", ep =>
        {
            ep.Description(x => x
                .Produces(401)
                .WithTags("Cart"));
        });
    }
}