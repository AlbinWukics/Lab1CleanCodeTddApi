using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Cart.Delete;

public class DeleteCartByIdEndpoint : Endpoint<DeleteCartByIdRequest, DeleteCartByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCartByIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Delete("/delete/{id}");
        AllowAnonymous();
        Group<CartApiGroup>();
    }

    public override async Task HandleAsync(DeleteCartByIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        var response = await _unitOfWork.CartRepository.RemoveAsync(guid);

        if (!response.Success || response.Data is null)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { CartDto = response.Data }, 200, ct);

    }
}