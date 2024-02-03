using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Customer.Get.GetCustomerById;

public class GetCustomerByIdEndpoint : Endpoint<GetCustomerByIdRequest, GetCustomerByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerByIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/get/{id}");
        AllowAnonymous();
        Group<CustomerApiGroup>();
    }

    public override async Task HandleAsync(GetCustomerByIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }
        
        var response = await _unitOfWork.CustomerRepository.GetByIdAsync(guid);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await SendAsync(new() { CustomerDto = response.Data }, 200, ct);

    }
}