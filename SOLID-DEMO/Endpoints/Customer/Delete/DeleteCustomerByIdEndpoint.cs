using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Customer.Delete;

public class DeleteCustomerByIdEndpoint : Endpoint<DeleteCustomerByIdRequest, DeleteCustomerByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerByIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Delete("/deleteById/{id}");
        AllowAnonymous();
        Group<CustomerApiGroup>();
    }

    public override async Task HandleAsync(DeleteCustomerByIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        var response = await _unitOfWork.CustomerRepository.RemoveAsync(guid);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { CustomerDto = response.Data }, 200, ct);
    }
}