using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Order.Put;

public class UpdateOrderEndpoint : Endpoint<UpdateOrderRequest, UpdateOrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Put("/update");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(UpdateOrderRequest req, CancellationToken ct)
    {
        var response = await _unitOfWork.OrderRepository.UpdateAsync(req.OrderDto);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { OrderDto = response.Data }, 200, ct);
    } 
}