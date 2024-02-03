using FastEndpoints;

namespace Server.Endpoints;

public sealed class ApiGroup : Group
{
    public ApiGroup()
    {
        Configure("/api", ep =>
        {
            ep.Description(x => x
                .Produces(401)
                .WithTags("API"));
        });
    }
}