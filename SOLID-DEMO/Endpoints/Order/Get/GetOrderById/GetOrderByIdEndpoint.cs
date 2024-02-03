using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Order.Get.GetOrderById;

public class GetOrderByIdEndpoint : Endpoint<GetOrderByIdRequest, GetOrderByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderByIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/GetById/{id}");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(GetOrderByIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        var response = await _unitOfWork.OrderRepository.GetByIdAsync(guid);

        if (!response.Success || response.Data is null)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await SendAsync(new() { OrderDto = response.Data }, 200, ct);
    } 
}