using FastEndpoints;
using Shared.Interfaces;

namespace Server.Endpoints.Cart.Post;

public class AddCartEndpoint : Endpoint<AddCartRequest, AddCartResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCartEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Post("/add");
        AllowAnonymous();
        Group<CartApiGroup>();
    }

    public override async Task HandleAsync(AddCartRequest req, CancellationToken ct)
    {
        var response = await _unitOfWork.CartRepository.AddAsync(req.CartDto);

        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { CartDto = response.Data }, 200, ct);
    }
}