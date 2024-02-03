using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Shared.DataTransferObjects;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;

public static class OrderDetailsConvertToExtensions
{
    public static OrderDetailsModel ConvertToModel(this OrderDetailsDto d)
    {
        return new OrderDetailsModel()
        {
            Id = d.Id,
            Product = d.Product.ConvertToModel(),
            AmountOfProducts = d.AmountOfProducts,
            Order = new OrderModel()
            {
                Id = d.Order.Id,
            }
        };
    }

    public static OrderDetailsDto ConvertToDto(this OrderDetailsModel m)
    {
        return new OrderDetailsDto()
        {
            Id = m.Id,
            Product = m.Product.ConvertToDto(),
            AmountOfProducts = m.AmountOfProducts,
            Order = new OrderDto()
            {
                Id = m.Order.Id,
            }
        };
    }
}