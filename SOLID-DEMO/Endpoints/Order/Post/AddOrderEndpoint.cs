using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Order.Post;

public class AddOrderEndpoint : Endpoint<AddOrderRequest, AddOrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddOrderEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Post("/add");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(AddOrderRequest req, CancellationToken ct)
    {
        var response = await _unitOfWork.OrderRepository.AddAsync(req.OrderDto);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { OrderDto = response.Data }, 200, ct);
    }
}