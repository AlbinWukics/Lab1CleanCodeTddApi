using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Cart.Get.GetCartsByCustomerId;

public class GetCartsByCustomerIdEndpoint : Endpoint<GetCartsByCustomerIdRequest, GetCartsByCustomerIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCartsByCustomerIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/GetByCustomerId/{id}");
        AllowAnonymous();
        Group<CartApiGroup>();
    }

    public override async Task HandleAsync(GetCartsByCustomerIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        var response = await _unitOfWork.CartRepository.GetByCustomerIdAsync(guid);

        if (!response.Success || response.Data is null)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await SendAsync(new() { Carts = response.Data }, 200, ct);
    }
}