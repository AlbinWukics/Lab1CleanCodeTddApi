using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Product.Get.GetProductById;

public class GetProductByIdEndpoint : Endpoint<GetProductByIdRequest, GetProductByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductByIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/GetById/{id}");
        AllowAnonymous();
        Group<ProductApiGroup>();
    }

    public override async Task HandleAsync(GetProductByIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        var response = await _unitOfWork.ProductRepository.GetByIdAsync(guid);

        if (!response.Success || response.Data is null)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await SendAsync(new() { ProductDto = response.Data }, 200, ct);
    }
}