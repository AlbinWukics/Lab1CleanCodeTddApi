using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Customer.Get.GetAllCustomers;

public class GetAllCustomersEndpoint : EndpointWithoutRequest<GetAllCustomersResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCustomersEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/getAll");
        AllowAnonymous();
        Group<CustomerApiGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _unitOfWork.CustomerRepository.GetAllAsync();

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

        await SendAsync(new() { Customers = response.Data }, 200, ct);
    }
}