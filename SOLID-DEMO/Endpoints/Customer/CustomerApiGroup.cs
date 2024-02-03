using FastEndpoints;

namespace Server.Endpoints.Customer;

public class CustomerApiGroup : SubGroup<ApiGroup>
{
    public CustomerApiGroup()
    {
        Configure("/customer", ep =>
        {
            ep.Description(x => x
                .Produces(401)
                .WithTags("Customer"));
        });
    }
}