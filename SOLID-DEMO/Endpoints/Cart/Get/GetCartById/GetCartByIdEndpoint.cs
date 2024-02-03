using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Cart.Get.GetCartById;

public class GetCartByIdEndpoint : Endpoint<GetCartByIdRequest, GetCartByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCartByIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/GetById/{id}");
        AllowAnonymous();
        Group<CartApiGroup>();
    }

    public override async Task HandleAsync(GetCartByIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        var response = await _unitOfWork.CartRepository.GetByIdAsync(guid);

        if (!response.Success || response.Data is null)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await SendAsync(new() { CartDto = response.Data }, 200, ct);

    }
}