using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Shared.DataTransferObjects;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;

public static class OrderConvertToExtensions
{
    public static OrderDto ConvertToDto(this OrderModel m)
    {
        return new OrderDto()
        {
            Id = m.Id,
            Customer = m.Customer.ConvertToDto(),
            CreatedDate = m.CreatedDate,
            ShippingDate = m.ShippingDate,
            UpdatedDate = m.UpdatedDate,
            OrderDetails = m.OrderDetails.Select(x => x.ConvertToDto()).ToList()
        };
    }

    public static OrderModel ConvertToModel(this OrderDto d)
    {
        return new OrderModel()
        {
            Id = d.Id,
            Customer = d.Customer.ConvertToModel(),
            CreatedDate = d.CreatedDate,
            ShippingDate = d.ShippingDate,
            UpdatedDate = d.UpdatedDate,
            OrderDetails = d.OrderDetails.Select(x => x.ConvertToModel()).ToList()
        };
    }
}