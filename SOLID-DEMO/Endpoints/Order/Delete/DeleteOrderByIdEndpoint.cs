using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Order.Delete;

public class DeleteOrderByIdEndpoint : Endpoint<DeleteOrderByIdRequest, DeleteOrderByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderByIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Delete("/delete/{id}");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(DeleteOrderByIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        var response = await _unitOfWork.OrderRepository.RemoveAsync(guid);

        if (!response.Success || response.Data is null)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { OrderDto = response.Data }, 200, ct);

    }
}