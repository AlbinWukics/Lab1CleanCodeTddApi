using FastEndpoints;

namespace Server.Endpoints.Product;

public class ProductApiGroup : SubGroup<ApiGroup>
{
    public ProductApiGroup()
    {
        Configure("/product", ep =>
        {
            ep.Description(x => x
                .Produces(401)
                .WithTags("Product"));
        });
    }
}