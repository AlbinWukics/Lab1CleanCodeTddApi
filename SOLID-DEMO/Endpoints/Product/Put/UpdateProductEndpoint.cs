using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Product.Put;

public class UpdateProductEndpoint : Endpoint<UpdateProductRequest, UpdateProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Put("/update");
        AllowAnonymous();
        Group<ProductApiGroup>();
    }

    public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
    {
        var response = await _unitOfWork.ProductRepository.UpdateAsync(req.ProductDto);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { ProductDto = response.Data }, 200, ct);
    }
}