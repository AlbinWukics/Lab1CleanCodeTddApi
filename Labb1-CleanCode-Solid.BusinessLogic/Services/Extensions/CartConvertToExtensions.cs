using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Shared.DataTransferObjects;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;

public static class CartConvertToExtensions
{
    public static CartDto ConvertToDto(this CartModel m)
    {
        return new CartDto()
        {
            Id = m.Id,
            CreatedDate = m.CreatedDate,
            UpdatedDate = m.UpdatedDate,
            Customer = m.Customer.ConvertToDto(),
            CartDetails = m.CartDetails.Select(x => x.ConvertToDto()).ToList()
        };
    }

    public static CartModel ConvertToModel(this CartDto d)
    {
        return new CartModel()
        {
            Id = d.Id,
            CreatedDate = d.CreatedDate,
            UpdatedDate = d.UpdatedDate,
            Customer = d.Customer.ConvertToModel(),
            CartDetails = d.CartDetails.Select(x => x.ConvertToModel()).ToList()
        };
    }


}