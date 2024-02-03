using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Cart.Put;

public class UpdateCartEndpoint : Endpoint<UpdateCartRequest, UpdateCartResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCartEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Put("/update");
        AllowAnonymous();
        Group<CartApiGroup>();
    }

    public override async Task HandleAsync(UpdateCartRequest req, CancellationToken ct)
    {
        var response = await _unitOfWork.CartRepository.UpdateAsync(req.CartDto);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { CartDto = response.Data }, 200, ct);
    }
}
