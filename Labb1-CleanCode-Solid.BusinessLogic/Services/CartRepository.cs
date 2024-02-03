using Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;
using Labb1_CleanCode_Solid.DataAccess.Contexts;
using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DataTransferObjects;
using Shared.Interfaces;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services;

public class CartRepository : ICartRepository
{
    private readonly ShopContext _ctx;
    public CartRepository(ShopContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ServiceResponse<CartDto>> AddAsync(CartDto dto)
    {
        // Future task: customer should only be able to hold one cart

        var validCustomer = await _ctx.Customer.FindAsync(dto.Customer.Id);
        if (validCustomer is null)
            return new ServiceResponse<CartDto>(false, null, "");

        foreach (var oD in dto.CartDetails)
        {
            oD.Id = Guid.NewGuid();
            oD.Cart = new CartDto() { Id = dto.Id };
        }

        var cartModel = dto.ConvertToModel();
        cartModel.Customer = validCustomer;

        foreach (var oD in cartModel.CartDetails)
        {
            var product = await _ctx.Product.FindAsync(oD.Product.Id);

            if (product is null)
            {
                // Product not found
                cartModel.CartDetails.Remove(oD);
                continue;
            }

            oD.Product = product;
        }

        cartModel.Id = Guid.NewGuid();
        cartModel.CreatedDate = DateTime.UtcNow;
        cartModel.UpdatedDate = DateTime.UtcNow;

        await _ctx.Cart.AddAsync(cartModel);

        return new ServiceResponse<CartDto>(true, cartModel.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<CartDto>> GetByIdAsync(Guid id)
    {
        var result = await _ctx.Cart
            .Include(x => x.CartDetails)
            .ThenInclude(x => x.Product)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (result is null)
            return new ServiceResponse<CartDto>(false, null, "");

        return new ServiceResponse<CartDto>(true, result.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<IEnumerable<CartDto>>> GetAllAsync()
    {
        var result = await _ctx.Cart.ToListAsync();
        return new ServiceResponse<IEnumerable<CartDto>>(true, result.Select(x => x.ConvertToDto()), "");
    }

    public async Task<ServiceResponse<CartDto>> UpdateAsync(CartDto dto)
    {
        var update = await _ctx.Cart
            .Include(x => x.CartDetails)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id.Equals(dto.Id));

        if (update is null)
            return new ServiceResponse<CartDto>(false, null, "");

        #region CartDetails
        update.CartDetails.Clear();

        foreach (var oD in dto.CartDetails)
        {
            var product = await _ctx.Product.FindAsync(oD.Product.Id);

            if (product is null) continue;

            update.CartDetails.Add(new CartDetailsModel()
            {
                Id = Guid.NewGuid(),
                AmountOfProducts = oD.AmountOfProducts,
                Product = product,
                Cart = new CartModel()
            });
        }

        if (update.CartDetails.Any())
            await _ctx.CartDetail.AddRangeAsync(update.CartDetails);
        #endregion

        update.UpdatedDate = DateTime.UtcNow;

        _ctx.Cart.Update(update);

        return new ServiceResponse<CartDto>(true, update.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<CartDto>> RemoveAsync(Guid id)
    {
        var cart = await _ctx.Cart
            .Include(x => x.CartDetails)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (cart is null)
            return new ServiceResponse<CartDto>(false, null, "");

        _ctx.Cart.Remove(cart);
        return new ServiceResponse<CartDto>(true, cart.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<IEnumerable<CartDto>>> GetByCustomerIdAsync(Guid customerId)
    {
        var carts = await _ctx.Cart
            .Include(x => x.CartDetails)
            .ThenInclude(x => x.Product)
            .Include(x => x.Customer)
            .Where(x => x.Customer.Id.Equals(customerId))
            .ToListAsync();

        return new ServiceResponse<IEnumerable<CartDto>>(true, carts.Select(x => x.ConvertToDto()), "");
    }
}