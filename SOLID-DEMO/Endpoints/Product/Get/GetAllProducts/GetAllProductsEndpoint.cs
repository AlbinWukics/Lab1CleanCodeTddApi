using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Product.Get.GetAllProducts;

public class GetAllProductsEndpoint : EndpointWithoutRequest<GetAllProductsResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllProductsEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/GetAll");
        AllowAnonymous();
        Group<ProductApiGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _unitOfWork.ProductRepository.GetAllAsync();

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

        await SendAsync(new() { Products = response.Data }, 200, ct);
    } 
}