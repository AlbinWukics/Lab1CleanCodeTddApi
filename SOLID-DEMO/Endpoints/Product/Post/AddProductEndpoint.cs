using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Product.Post;

public class AddProductEndpoint : Endpoint<AddProductRequest, AddProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddProductEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Post("/add");
        AllowAnonymous();
        Group<ProductApiGroup>();
    }

    public override async Task HandleAsync(AddProductRequest req, CancellationToken ct)
    {
        var response = await _unitOfWork.ProductRepository.AddAsync(req.ProductDto);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { ProductDto = response.Data }, 200, ct);
    } 
}