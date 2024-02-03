using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Cart.Get.GetAllCarts;

public class GetAllCartsEndpoint : EndpointWithoutRequest<GetAllCartsResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCartsEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/GetAll");
        AllowAnonymous();
        Group<CartApiGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _unitOfWork.CartRepository.GetAllAsync();

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        if (response.Data is null)
        {
            await SendAsync(new(), 204, ct);
            return;
        }

        await SendAsync(new() { Carts = response.Data }, 200, ct);
    }
}