using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Shared.DataTransferObjects;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;

public static class ProductConvertToExtensions
{
    public static ProductDto ConvertToDto(this ProductModel m)
    {
        return new ProductDto()
        {
            Id = m.Id,
            Name = m.Name,
            CreatedDate = m.CreatedDate,
            Description = m.Description,
            LastUpdatedDate = m.LastUpdatedDate,
            UnitPrice = m.UnitPrice
        };
    }

    public static ProductModel ConvertToModel(this ProductDto dto)
    {
        return new ProductModel()
        {
            Id = dto.Id,
            Name = dto.Name,
            CreatedDate = dto.CreatedDate,
            Description = dto.Description,
            LastUpdatedDate = dto.LastUpdatedDate,
            UnitPrice = dto.UnitPrice
        };
    }
}