using Shared;

namespace Labb1_CleanCode_Solid.BusinessLogic.Tests;

/// <summary>
/// Sr means ServiceResponse
/// </summary>
public class CustomerRepository_Tests
{
    #region InMemoryDb
    private static async Task<ShopContext> CreateShopContextWithInMemoryDbAsync()
    {
        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;

        var context = new ShopContext(options);

        context.Customer.AddRange(
            new CustomerModel() { Id = Guid.NewGuid(), Email = "hej@albin.se", Password = "lösenord1" },
            new CustomerModel() { Id = Guid.NewGuid(), Email = "hej2@albin.se", Password = "lösenord2" }
        );

        await context.SaveChangesAsync();

        return context;
    }
    #endregion

    #region AddAsync
    [Fact]
    public async Task CustomerRepository_AddAsync_ReturnServiceResponseWithTrueAndCustomerDto()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var customer = new CustomerDto() { Email = "test@test.test", Password = "hej" };
        var expected = true;

        // Act
        var result = await sut.AddAsync(customer);
        await context.SaveChangesAsync();
        var secondResult = await context.Customer.FirstOrDefaultAsync(x => x.Email.Equals("test@test.test"));

        // Assert
        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);

        Assert.NotNull(secondResult);
        Assert.Equal(result.Data.Id, secondResult.Id);
    }
    #endregion

    #region GetByIdAsync
    [Fact]
    public async Task CustomerRepository_GetByIdAsync_CustomerIdNotFoundReturnsSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var guid = Guid.NewGuid();
        var expected = false;


        // Act
        var result = await sut.GetByIdAsync(guid);

        // Assert
        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CustomerRepository_GetByIdAsync_CustomerIdFoundReturnsSrWithTrue()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var guid = await context.Customer.Select(x => x.Id).FirstOrDefaultAsync();
        var expected = true;


        // Act
        var result = await sut.GetByIdAsync(guid);

        // Assert
        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region GetAllAsync
    [Fact]
    public async Task CustomerRepository_GetAllAsync_ReturnSrWithIEnumerableOfCustomerDtoWithTrue()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var expected = true;

        // Act
        var result = await sut.GetAllAsync();

        // Assert
        Assert.IsType<ServiceResponse<IEnumerable<CustomerDto>>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region UpdateAsync
    [Fact]
    public async Task CustomerRepository_UpdateAsync_ShouldNotFindCustomerToUpdateReturnSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var expected = false;
        var customer = new CustomerDto() { Id = Guid.NewGuid() };

        // Act
        var result = await sut.UpdateAsync(customer);

        // Assert
        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CustomerRepository_UpdateAsync_ReturnSrWithUpdatedCustomer()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var expected = true;
        var model = await context.Customer.FirstAsync();
        var customerDto = new CustomerDto() { Id = model.Id, Email = "newEmail@new.com" };


        // Act
        var result = await sut.UpdateAsync(customerDto);
        await context.SaveChangesAsync();
        var secondResult = await context.Customer.FindAsync(model.Id);


        // Assert
        Assert.NotNull(secondResult);
        Assert.Equal(secondResult.Email, customerDto.Email);

        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region RemoveAsync
    [Fact]
    public async Task CustomerRepository_RemoveAsync_ShouldNotFindCustomerToRemoveReturnSrWithFalse()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var expected = false;
        var id = Guid.NewGuid();

        // Act
        var result = await sut.RemoveAsync(id);

        // Arrange
        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CustomerRepository_RemoveAsync_ReturnSrWithTrueAndRemovedCustomer()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var expected = true;
        var model = await context.Customer.FirstAsync();

        // Act
        var result = await sut.RemoveAsync(model.Id);
        await context.SaveChangesAsync();
        var secondResult = await context.Customer.FindAsync(model.Id);

        // Arrange
        Assert.NotNull(model);

        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);

        Assert.Null(secondResult);
    }
    #endregion

    #region LoginCustomerAsync
    [Fact]
    public async Task CustomerRepository_LoginCustomer_FailLoginWrongEmailReturnSrWithFalse()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var expected = false;
        var model = await context.Customer.FirstAsync();
        var wrongEmail = "wrongEmail@hej.com";

        // Act
        var result = await sut.LoginCustomerAsync(wrongEmail, model.Password);

        // Arrange
        Assert.NotNull(model);

        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CustomerRepository_LoginCustomer_FailLoginWrongPasswordReturnSrWithFalse()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var expected = false;
        var model = await context.Customer.FirstAsync();
        var wrongPassword = "wrongPassword";

        // Act
        var result = await sut.LoginCustomerAsync(model.Email, wrongPassword);

        // Arrange
        Assert.NotNull(model);

        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CustomerRepository_LoginCustomer_ReturnSrWithCustomerDtoAndTrue()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CustomerRepository(context);
        var expected = true;
        var model = await context.Customer.FirstAsync();

        // Act
        var result = await sut.LoginCustomerAsync(model.Email, model.Password);

        // Arrange
        Assert.NotNull(model);

        Assert.IsType<ServiceResponse<CustomerDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion
}