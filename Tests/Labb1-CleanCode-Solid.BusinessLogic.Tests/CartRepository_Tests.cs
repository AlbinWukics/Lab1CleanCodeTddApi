using Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;
using Shared;

namespace Labb1_CleanCode_Solid.BusinessLogic.Tests;

/// <summary>
/// Sr means ServiceResponse
/// </summary>
public class CartRepository_Tests
{
    #region InMemoryDb
    private static async Task<ShopContext> CreateShopContextWithInMemoryDbAsync()
    {
        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;

        var context = new ShopContext(options);

        context.Product.AddRange(
            new ProductModel() { Id = Guid.NewGuid(), Name = "productOne", UnitPrice = 5.5m },
            new ProductModel() { Id = Guid.NewGuid(), Name = "productTwo", UnitPrice = 1.2m });

        context.Customer.AddRange(
            new CustomerModel() { Id = Guid.NewGuid(), Email = "hej@albin.se", Password = "lösenord1" },
            new CustomerModel() { Id = Guid.NewGuid(), Email = "hej2@albin.se", Password = "lösenord2" });

        await context.SaveChangesAsync();

        return context;
    }
    #endregion

    #region AddAsync
    [Fact]
    public async Task CartRepository_AddAsync_InvalidCustomerReturnSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var expected = false;
        var customer = new CustomerDto() { Id = Guid.NewGuid() };
        var cart = new CartDto() { Customer = customer, CartDetails = new List<CartDetailsDto>() };


        // Act
        var result = await sut.AddAsync(cart);

        // Assert
        Assert.IsType<ServiceResponse<CartDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CartRepository_AddAsync_ReturnSrWithTrueAndCartDto()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var expected = true;

        var product = await context.Product.FirstAsync();
        var customer = await context.Customer.FirstAsync();

        var cartDetails = new List<CartDetailsDto>()
        {
            new CartDetailsDto() { Product = product.ConvertToDto() }
        };

        var cart = new CartDto() { Customer = customer.ConvertToDto(), CartDetails = cartDetails };

        // Act
        var result = await sut.AddAsync(cart);
        await context.SaveChangesAsync();
        var secondResult = await context.Cart
            .Include(x => x.Customer)
            .Include(x => x.CartDetails)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id.Equals(result.Data!.Id));

        // Assert
        Assert.NotNull(product);
        Assert.NotNull(customer);
        Assert.NotNull(result.Data);
        Assert.NotNull(secondResult);

        Assert.Equal(secondResult.Customer.Id, customer.Id);
        Assert.Equal(secondResult.CartDetails.First().Product.Id, product.Id);

        Assert.IsType<ServiceResponse<CartDto>>(result);
        Assert.Equal(expected, result.Success);

        Assert.Equal(result.Data.Id, secondResult.Id);
    }
    #endregion

    #region GetByIdAsync
    [Fact]
    public async Task CartRepository_GetByIdAsync_CartIdNotFoundReturnsSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var guid = Guid.NewGuid();
        var expected = false;


        // Act
        var result = await sut.GetByIdAsync(guid);

        // Assert
        Assert.IsType<ServiceResponse<CartDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CartRepository_GetByIdAsync_CartIdFoundReturnsSrWithTrue()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var expected = true;

        var product = await context.Product.FirstAsync();
        var customer = await context.Customer.FirstAsync();

        var cartDetails = new List<CartDetailsDto>()
        {
            new CartDetailsDto() { Product = product.ConvertToDto() }
        };

        var cart = new CartDto() { Customer = customer.ConvertToDto(), CartDetails = cartDetails };

        // Act
        await sut.AddAsync(cart);
        await context.SaveChangesAsync();
        var guid = await context.Cart.Select(x => x.Id).FirstOrDefaultAsync();
        var result = await sut.GetByIdAsync(guid);

        // Assert
        Assert.NotNull(product);
        Assert.NotNull(customer);
        Assert.NotNull(result.Data);

        Assert.IsType<ServiceResponse<CartDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region GetAllAsync
    [Fact]
    public async Task CartRepository_GetAllAsync_ReturnSrWithIEnumerableOfCartDtoWithTrue()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var expected = true;

        // Act
        var result = await sut.GetAllAsync();

        // Assert
        Assert.IsType<ServiceResponse<IEnumerable<CartDto>>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region UpdateAsync
    [Fact]
    public async Task CartRepository_UpdateAsync_ShouldNotFindCartToUpdateReturnSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var expected = false;
        var cart = new CartDto() { Id = Guid.NewGuid() };

        // Act
        var result = await sut.UpdateAsync(cart);

        // Assert
        Assert.IsType<ServiceResponse<CartDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CartRepository_UpdateAsync_ReturnSrWithUpdatedCart()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var expected = true;
        var oldAmount = 1;
        var newAmount = 2;

        var product = await context.Product.FirstAsync();
        var customer = await context.Customer.FirstAsync();
        var cartDetails = new List<CartDetailsDto>()
        {
            new CartDetailsDto() { Product = product.ConvertToDto(), AmountOfProducts = oldAmount}
        };
        var cart = new CartDto() { Customer = customer.ConvertToDto(), CartDetails = cartDetails };

        // Act
        await sut.AddAsync(cart);
        await context.SaveChangesAsync();
        var firstResult = await context.Cart
            .Include(x => x.CartDetails)
            .FirstAsync();

        firstResult!.CartDetails.ToList()[0].AmountOfProducts = newAmount;
        context.Entry(firstResult).State = EntityState.Detached;

        var secondResult = await sut.UpdateAsync(firstResult.ConvertToDto());
        await context.SaveChangesAsync();

        var thirdResult = await context.Cart
            .Include(x => x.CartDetails)
            .FirstOrDefaultAsync(x => x.Id.Equals(secondResult.Data!.Id));

        // Assert
        Assert.NotNull(firstResult);
        Assert.NotNull(secondResult.Data);
        Assert.NotNull(thirdResult);
        Assert.Equal(newAmount, thirdResult.CartDetails.ToList()[0].AmountOfProducts);

        Assert.IsType<ServiceResponse<CartDto>>(secondResult);
        Assert.Equal(expected, secondResult.Success);
    }
    #endregion

    #region RemoveAsync
    [Fact]
    public async Task CartRepository_RemoveAsync_ShouldNotFindCartToRemoveReturnSrWithFalse()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var expected = false;
        var id = Guid.NewGuid();

        // Act
        var result = await sut.RemoveAsync(id);

        // Arrange
        Assert.IsType<ServiceResponse<CartDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CartRepository_RemoveAsync_ReturnSrWithTrueAndRemovedCart()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var expected = true;
        var guid = Guid.NewGuid();

        var cart = new CartModel() { Id = guid, Customer = new CustomerModel() { Email = "a@a.a", Password = "lösenord1" }, CartDetails = new List<CartDetailsModel>() };

        // Act
        await context.AddAsync(cart);
        await context.SaveChangesAsync();
        var result = await sut.RemoveAsync(guid);
        await context.SaveChangesAsync();
        var secondResult = await context.Cart.FindAsync(result.Data?.Id);

        // Arrange
        Assert.NotNull(result.Data);
        Assert.Null(secondResult);
        Assert.IsType<ServiceResponse<CartDto>>(result);
        Assert.Equal(expected, result.Success);
    }
    #endregion

    #region GetByCustomerIdAsync
    [Fact]
    public async Task CartRepository_GetByCustomerIdAsync_ReturnSrWithListOfCartDto()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new CartRepository(context);
        var expected = true;

        var firstGuid = Guid.NewGuid();
        var secondGuid = Guid.NewGuid();
        var cart = new CartModel() { Id = firstGuid, Customer = new CustomerModel() { Id = secondGuid, Email = "a@a.a", Password = "lösenord1" }, CartDetails = new List<CartDetailsModel>() };

        // Act
        await context.Cart.AddAsync(cart);
        await context.SaveChangesAsync();
        var result = await sut.GetByCustomerIdAsync(secondGuid);

        // Assert
        Assert.IsType<ServiceResponse<IEnumerable<CartDto>>>(result);
        Assert.Equal(expected, result.Success);

        Assert.NotNull(result.Data);
        Assert.Equal(result.Data.First().Id, firstGuid);
        Assert.Equal(result.Data.First().Customer.Id, secondGuid);
    }
    #endregion
}