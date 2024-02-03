using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Order.Get.GetOrdersByCustomerId;

public class GetOrdersByCustomerIdEndpoint : Endpoint<GetOrdersByCustomerIdRequest, GetOrdersByCustomerIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrdersByCustomerIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/GetByCustomerId/{id}");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(GetOrdersByCustomerIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        var response = await _unitOfWork.OrderRepository.GetByCustomerIdAsync(guid);

        if (!response.Success || response.Data is null)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await SendAsync(new() { Orders = response.Data }, 200, ct);
    }
}