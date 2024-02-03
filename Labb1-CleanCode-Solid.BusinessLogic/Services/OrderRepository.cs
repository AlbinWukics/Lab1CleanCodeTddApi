using Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;
using Labb1_CleanCode_Solid.DataAccess.Contexts;
using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DataTransferObjects;
using Shared.Interfaces;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services;

public class OrderRepository : IOrderRepository
{
    private readonly ShopContext _ctx;

    public OrderRepository(ShopContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ServiceResponse<OrderDto>> AddAsync(OrderDto dto)
    {
        var validCustomer = await _ctx.Customer.FindAsync(dto.Customer.Id);
        if (validCustomer is null)
            return new ServiceResponse<OrderDto>(false, null, "");

        // Two loops because OrderDto must be an instance in ConvertToModel & second loop must be a model to allow EF tracking
        foreach (var oD in dto.OrderDetails)
        {
            oD.Id = Guid.NewGuid();
            oD.Order = new OrderDto() { Id = dto.Id };
        }

        var orderModel = dto.ConvertToModel();
        orderModel.Customer = validCustomer;

        foreach (var oD in orderModel.OrderDetails)
        {
            var product = await _ctx.Product.FindAsync(oD.Product.Id);

            if (product is null)
            {
                // Product not found
                orderModel.OrderDetails.Remove(oD);
                continue;
            }

            oD.Product = product;
        }

        orderModel.Id = Guid.NewGuid();
        orderModel.CreatedDate = DateTime.UtcNow;
        orderModel.UpdatedDate = DateTime.UtcNow;


        await _ctx.Order.AddAsync(orderModel);

        return new ServiceResponse<OrderDto>(true, orderModel.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<OrderDto>> GetByIdAsync(Guid id)
    {
        var result = await _ctx.Order
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (result is null)
            return new ServiceResponse<OrderDto>(false, null, "");

        return new ServiceResponse<OrderDto>(true, result.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<IEnumerable<OrderDto>>> GetAllAsync()
    {
        var result = await _ctx.Order.ToListAsync();
        return new ServiceResponse<IEnumerable<OrderDto>>(true, result.Select(x => x.ConvertToDto()), "");
    }

    public async Task<ServiceResponse<OrderDto>> UpdateAsync(OrderDto dto)
    {
        var update = await _ctx.Order
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id.Equals(dto.Id));

        if (update is null)
            return new ServiceResponse<OrderDto>(false, null, "");

        #region OrderDetails
        update.OrderDetails.Clear();

        foreach (var oD in dto.OrderDetails)
        {
            var product = await _ctx.Product.FindAsync(oD.Product.Id);

            if (product is null) continue;

            update.OrderDetails.Add(new OrderDetailsModel()
            {
                Id = Guid.NewGuid(),
                AmountOfProducts = oD.AmountOfProducts,
                Product = product,
                Order = new OrderModel()
            });
        }

        if (update.OrderDetails.Any())
            await _ctx.OrderDetail.AddRangeAsync(update.OrderDetails);
        #endregion

        update.ShippingDate = dto.ShippingDate;
        update.UpdatedDate = DateTime.UtcNow;

        _ctx.Order.Update(update);

        return new ServiceResponse<OrderDto>(true, update.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<OrderDto>> RemoveAsync(Guid id)
    {
        var order = await _ctx.Order
            .Include(x => x.OrderDetails)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (order is null)
            return new ServiceResponse<OrderDto>(false, null, "");

        _ctx.Order.Remove(order);
        return new ServiceResponse<OrderDto>(true, order.ConvertToDto(), "");
    }

    public async Task<ServiceResponse<IEnumerable<OrderDto>>> GetByCustomerIdAsync(Guid customerId)
    {
        var orders = await _ctx.Order
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .Include(x => x.Customer)
            .Where(x => x.Customer.Id.Equals(customerId))
            .ToListAsync();

        return new ServiceResponse<IEnumerable<OrderDto>>(true, orders.Select(x => x.ConvertToDto()), "");
    }
}