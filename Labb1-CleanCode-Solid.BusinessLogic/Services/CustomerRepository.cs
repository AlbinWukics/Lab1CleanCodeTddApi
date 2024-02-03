using Labb1_CleanCode_Solid.DataAccess.Contexts;
using Labb1_CleanCode_Solid.DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DataTransferObjects;
using Shared.Interfaces;

namespace Labb1_CleanCode_Solid.BusinessLogic.Services;

public class CustomerRepository : ICustomerRepository
{
    private readonly ShopContext _ctx;

    public CustomerRepository(ShopContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ServiceResponse<CustomerDto>> AddAsync(CustomerDto dto)
    {
        dto.Id = Guid.NewGuid();
        dto.CreatedDate = DateTime.UtcNow;
        dto.UpdatedDate = DateTime.UtcNow;

        await _ctx.Customer.AddAsync(ConvertToModel(dto));

        return new ServiceResponse<CustomerDto>(true, dto, "");
    }

    public async Task<ServiceResponse<CustomerDto>> GetByIdAsync(Guid id)
    {
        var result = await _ctx.Customer.FindAsync(id);

        if (result is null)
            return new ServiceResponse<CustomerDto>(false, null, "");

        return new ServiceResponse<CustomerDto>(true, ConvertToDto(result), "");
    }

    public async Task<ServiceResponse<IEnumerable<CustomerDto>>> GetAllAsync()
    {
        var result = await _ctx.Customer.ToListAsync();
        return new ServiceResponse<IEnumerable<CustomerDto>>(true, result.Select(ConvertToDto), "");
    }

    public async Task<ServiceResponse<CustomerDto>> UpdateAsync(CustomerDto dto)
    {
        var update = await _ctx.Customer.FindAsync(dto.Id);

        if (update is null)
            return new ServiceResponse<CustomerDto>(false, null, "");

        update.Email = dto.Email;
        update.Password = dto.Password;
        update.UpdatedDate = DateTime.UtcNow;
        update.Name = dto.Name;

        _ctx.Customer.Update(update);

        return new ServiceResponse<CustomerDto>(true, ConvertToDto(update), "");
    }

    public async Task<ServiceResponse<CustomerDto>> RemoveAsync(Guid id)
    {
        var customer = await _ctx.Customer.FindAsync(id);

        if (customer is null)
            return new ServiceResponse<CustomerDto>(false, null, "");

        _ctx.Customer.Remove(customer);
        return new ServiceResponse<CustomerDto>(true, ConvertToDto(customer), "");
    }

    public async Task<ServiceResponse<CustomerDto>> LoginCustomerAsync(string email, string password)
    {
        var customer = await _ctx.Customer
            .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));

        if (customer is null)
            return new ServiceResponse<CustomerDto>(false, null, "");

        return customer.Password.Equals(password)
            ? new ServiceResponse<CustomerDto>(true, ConvertToDto(customer), "")
            : new ServiceResponse<CustomerDto>(false, null, "");
    }

    private static CustomerModel ConvertToModel(CustomerDto dto)
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

    private static CustomerDto ConvertToDto(CustomerModel model)
    {
        return new CustomerDto()
        {
            Id = model.Id,
            CreatedDate = model.CreatedDate,
            Email = model.Email,
            Name = model.Name,
            Password = model.Password,
            UpdatedDate = model.UpdatedDate
        };
    }
}