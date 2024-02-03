using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Shared.DataTransferObjects;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;

public static class CustomerConvertToExtensions
{
    public static CustomerModel ConvertToModel(this CustomerDto dto)
    {
        return new CustomerModel()
        {
            Id = dto.Id,
            CreatedDate = DateTime.UtcNow,
            Email = dto.Email,
            Name = dto.Name,
            Password = dto.Password,
            UpdatedDate = dto.UpdatedDate
        };
    }

    public static CustomerDto ConvertToDto(this CustomerModel m)
    {
        return new CustomerDto()
        {
            Id = m.Id,
            CreatedDate = m.CreatedDate,
            Email = m.Email,
            Name = m.Name,
            Password = m.Password,
            UpdatedDate = m.UpdatedDate
        };
    }
}