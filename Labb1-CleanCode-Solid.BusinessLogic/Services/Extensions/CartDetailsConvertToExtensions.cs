using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Shared.DataTransferObjects;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;

public static class CartDetailsConvertToExtensions
{
    public static CartDetailsDto ConvertToDto(this CartDetailsModel m)
    {
        return new CartDetailsDto()
        {
            Id = m.Id,
            AmountOfProducts = m.AmountOfProducts,
            Product = m.Product.ConvertToDto(),
            Cart = new CartDto() { Id = m.Id }
        };
    }

    public static CartDetailsModel ConvertToModel(this CartDetailsDto d)
    {
        return new CartDetailsModel()
        {
            Id = d.Id,
            AmountOfProducts = d.AmountOfProducts,
            Product = d.Product.ConvertToModel(),
            Cart = new CartModel() { Id = d.Id }
        };
    }
}