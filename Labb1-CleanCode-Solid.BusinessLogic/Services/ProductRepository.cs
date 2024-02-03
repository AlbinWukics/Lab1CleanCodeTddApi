using Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;
using Labb1_CleanCode_Solid.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DataTransferObjects;
using Shared.Interfaces;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services;

public class ProductRepository : IProductRepository
{
    private readonly ShopContext _ctx;

    public ProductRepository(ShopContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ServiceResponse<ProductDto>> AddAsync(ProductDto dto)
    {
        dto.Id = Guid.NewGuid();
        dto.CreatedDate = DateTime.UtcNow;
        dto.LastUpdatedDate = DateTime.UtcNow;

        await _ctx.Product.AddAsync(dto.ConvertToModel());
        return new ServiceResponse<ProductDto>(true, dto, "");
    }

    public async Task<ServiceResponse<ProductDto>> GetByIdAsync(Guid id)
    {
        var result = await _ctx.Product.FindAsync(id);

        if (result is null)
            return new ServiceResponse<ProductDto>(false, null, "");

        return new ServiceResponse<ProductDto>(true, result.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<IEnumerable<ProductDto>>> GetAllAsync()
    {
        var result = await _ctx.Product.ToListAsync();
        return new ServiceResponse<IEnumerable<ProductDto>>(true, result.Select(x => x.ConvertToDto()), "");
    }

    public async Task<ServiceResponse<ProductDto>> UpdateAsync(ProductDto dto)
    {
        var update = await _ctx.Product.FindAsync(dto.Id);

        if (update is null)
            return new ServiceResponse<ProductDto>(false, null, "");

        update.Name = dto.Name;
        update.Description = dto.Description;
        update.UnitPrice = dto.UnitPrice;
        update.LastUpdatedDate = DateTime.UtcNow;

        _ctx.Product.Update(update);

        return new ServiceResponse<ProductDto>(true, update.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<ProductDto>> RemoveAsync(Guid id)
    {
        var product = await _ctx.Product.FindAsync(id);

        if (product is null)
            return new ServiceResponse<ProductDto>(false, null, "");

        _ctx.Product.Remove(product);
        return new ServiceResponse<ProductDto>(true, product.ConvertToDto(), "");
    }
}

