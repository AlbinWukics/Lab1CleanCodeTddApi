using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Product.Delete;

public class DeleteProductByIdEndpoint : Endpoint<DeleteProductByIdRequest, DeleteProductByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductByIdEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Post("/delete/{id}");
        AllowAnonymous();
        Group<ProductApiGroup>();
    }

    public override async Task HandleAsync(DeleteProductByIdRequest req, CancellationToken ct)
    {
        if (!Guid.TryParse(req.Id, out var guid))
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        var response = await _unitOfWork.ProductRepository.RemoveAsync(guid);

        if (!response.Success || response.Data is null)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { ProductDto = response.Data }, 200, ct);

    }
}