using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Customer.Put;

public class UpdateCustomerEndpoint : Endpoint<UpdateCustomerRequest, UpdateCustomerResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Put("/update");
        AllowAnonymous();
        Group<CustomerApiGroup>();
    }

    public override async Task HandleAsync(UpdateCustomerRequest req, CancellationToken ct)
    {
        var response = await _unitOfWork.CustomerRepository.UpdateAsync(req.CustomerDto);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { CustomerDto = response.Data }, 200, ct);

    }
}