using Labb1_CleanCode_Solid.BusinessLogic.Services.Extensions;
using Shared;

namespace Labb1_CleanCode_Solid.BusinessLogic.Tests;

/// <summary>
/// Sr means ServiceResponse
/// </summary>
public class OrderRepository_Tests
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
    public async Task OrderRepository_AddAsync_InvalidCustomerReturnSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var expected = false;
        var customer = new CustomerDto() { Id = Guid.NewGuid() };
        var order = new OrderDto() { Customer = customer, OrderDetails = new List<OrderDetailsDto>() };


        // Act
        var result = await sut.AddAsync(order);

        // Assert
        Assert.IsType<ServiceResponse<OrderDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task OrderRepository_AddAsync_ReturnSrWithTrueAndOrderDto()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var expected = true;

        var product = await context.Product.FirstAsync();
        var customer = await context.Customer.FirstAsync();

        var orderDetails = new List<OrderDetailsDto>()
        {
            new OrderDetailsDto() { Product = product.ConvertToDto() }
        };

        var order = new OrderDto() { Customer = customer.ConvertToDto(), OrderDetails = orderDetails };

        // Act
        var result = await sut.AddAsync(order);
        await context.SaveChangesAsync();
        var secondResult = await context.Order
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .FirstAsync(x => x.Id.Equals(result.Data!.Id));

        // Assert
        Assert.NotNull(product);
        Assert.NotNull(customer);
        Assert.NotNull(result.Data);
        Assert.NotNull(secondResult);

        Assert.Equal(secondResult.Customer.Id, customer.Id);
        Assert.Equal(secondResult.OrderDetails.First().Product.Id, product.Id);

        Assert.IsType<ServiceResponse<OrderDto>>(result);
        Assert.Equal(expected, result.Success);

        Assert.Equal(result.Data.Id, secondResult.Id);
    }
    #endregion

    #region GetByIdAsync
    [Fact]
    public async Task OrderRepository_GetByIdAsync_OrderIdNotFoundReturnsSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var guid = Guid.NewGuid();
        var expected = false;


        // Act
        var result = await sut.GetByIdAsync(guid);

        // Assert
        Assert.IsType<ServiceResponse<OrderDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task OrderRepository_GetByIdAsync_OrderIdFoundReturnsSrWithTrue()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var expected = true;

        var product = await context.Product.FirstAsync();
        var customer = await context.Customer.FirstAsync();

        var orderDetails = new List<OrderDetailsDto>()
        {
            new OrderDetailsDto() { Product = product.ConvertToDto() }
        };

        var order = new OrderDto() { Customer = customer.ConvertToDto(), OrderDetails = orderDetails };

        // Act
        await sut.AddAsync(order);
        await context.SaveChangesAsync();
        var guid = await context.Order.Select(x => x.Id).FirstOrDefaultAsync();
        var result = await sut.GetByIdAsync(guid);

        // Assert
        Assert.NotNull(product);
        Assert.NotNull(customer);
        Assert.NotNull(result.Data);

        Assert.IsType<ServiceResponse<OrderDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region GetAllAsync
    [Fact]
    public async Task OrderRepository_GetAllAsync_ReturnSrWithIEnumerableOfOrderDtoWithTrue()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var expected = true;

        // Act
        var result = await sut.GetAllAsync();

        // Assert
        Assert.IsType<ServiceResponse<IEnumerable<OrderDto>>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region UpdateAsync
    [Fact]
    public async Task OrderRepository_UpdateAsync_ShouldNotFindOrderToUpdateReturnSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var expected = false;
        var order = new OrderDto() { Id = Guid.NewGuid() };

        // Act
        var result = await sut.UpdateAsync(order);

        // Assert
        Assert.IsType<ServiceResponse<OrderDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task OrderRepository_UpdateAsync_ReturnSrWithUpdatedOrder()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var expected = true;
        var oldAmount = 1;
        var newAmount = 2;

        var product = await context.Product.FirstAsync();
        var customer = await context.Customer.FirstAsync();
        var orderDetails = new List<OrderDetailsDto>()
        {
            new OrderDetailsDto() { Product = product.ConvertToDto(), AmountOfProducts = oldAmount}
        };
        var order = new OrderDto() { Customer = customer.ConvertToDto(), OrderDetails = orderDetails };

        // Act
        await sut.AddAsync(order);
        await context.SaveChangesAsync();
        var firstResult = await context.Order
            .Include(x => x.OrderDetails)
            .FirstAsync();

        firstResult!.OrderDetails.ToList()[0].AmountOfProducts = newAmount;
        context.Entry(firstResult).State = EntityState.Detached;

        var secondResult = await sut.UpdateAsync(firstResult.ConvertToDto());
        await context.SaveChangesAsync();

        var thirdResult = await context.Order
            .Include(x => x.OrderDetails)
            .FirstOrDefaultAsync(x => x.Id.Equals(secondResult.Data!.Id));

        // Assert
        Assert.NotNull(firstResult);
        Assert.NotNull(secondResult.Data);
        Assert.NotNull(thirdResult);
        Assert.Equal(newAmount, thirdResult.OrderDetails.ToList()[0].AmountOfProducts);

        Assert.IsType<ServiceResponse<OrderDto>>(secondResult);
        Assert.Equal(expected, secondResult.Success);
    }
    #endregion

    #region RemoveAsync
    [Fact]
    public async Task OrderRepository_RemoveAsync_ShouldNotFindOrderToRemoveReturnSrWithFalse()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var expected = false;
        var id = Guid.NewGuid();

        // Act
        var result = await sut.RemoveAsync(id);

        // Arrange
        Assert.IsType<ServiceResponse<OrderDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task OrderRepository_RemoveAsync_ReturnSrWithTrueAndRemovedOrder()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var expected = true;
        var guid = Guid.NewGuid();

        var order = new OrderModel() { Id = guid, Customer = new CustomerModel() { Email = "a@a.a", Password = "lösenord1" }, OrderDetails = new List<OrderDetailsModel>() };

        // Act
        await context.AddAsync(order);
        await context.SaveChangesAsync();
        var result = await sut.RemoveAsync(guid);
        await context.SaveChangesAsync();
        var secondResult = await context.Order.FindAsync(result.Data?.Id);

        // Arrange
        Assert.NotNull(result.Data);
        Assert.Null(secondResult);
        Assert.IsType<ServiceResponse<OrderDto>>(result);
        Assert.Equal(expected, result.Success);
    }
    #endregion

    #region GetByCustomerIdAsync
    [Fact]
    public async Task OrderRepository_GetByCustomerIdAsync_ReturnSrWithListOfOrderDto()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new OrderRepository(context);
        var expected = true;

        var firstGuid = Guid.NewGuid();
        var secondGuid = Guid.NewGuid();
        var order = new OrderModel() { Id = firstGuid, Customer = new CustomerModel() { Id = secondGuid, Email = "a@a.a", Password = "lösenord1" }, OrderDetails = new List<OrderDetailsModel>() };

        // Act
        await context.Order.AddAsync(order);
        await context.SaveChangesAsync();
        var result = await sut.GetByCustomerIdAsync(secondGuid);

        // Assert
        Assert.IsType<ServiceResponse<IEnumerable<OrderDto>>>(result);
        Assert.Equal(expected, result.Success);

        Assert.NotNull(result.Data);
        Assert.Equal(result.Data.First().Id, firstGuid);
        Assert.Equal(result.Data.First().Customer.Id, secondGuid);
    }
    #endregion
}