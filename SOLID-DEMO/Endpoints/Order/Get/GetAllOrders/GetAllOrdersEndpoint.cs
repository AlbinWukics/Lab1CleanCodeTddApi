using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Order.Get.GetAllOrders;

public class GetAllOrdersEndpoint : EndpointWithoutRequest<GetAllOrdersResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllOrdersEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/GetAll");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _unitOfWork.OrderRepository.GetAllAsync();

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

        await SendAsync(new() { Orders = response.Data }, 200, ct);
    }   
}