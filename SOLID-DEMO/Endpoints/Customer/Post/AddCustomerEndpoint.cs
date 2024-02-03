using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Customer.Post;

public class AddCustomerEndpoint : Endpoint<AddCustomerRequest, AddCustomerResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCustomerEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Post("/add");
        AllowAnonymous();
        Group<CustomerApiGroup>();
    }

    public override async Task HandleAsync(AddCustomerRequest req, CancellationToken ct)
    {
        var response = await _unitOfWork.CustomerRepository.AddAsync(req.CustomerDto);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { CustomerDto = response.Data }, 200, ct);
    }
}