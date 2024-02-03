using Shared;

namespace Labb1_CleanCode_Solid.BusinessLogic.Tests;

/// <summary>
/// Sr means ServiceResponse
/// </summary>
public class ProductRepository_Tests
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
            new ProductModel() { Id = Guid.NewGuid(), Name = "productTwo", UnitPrice = 1.2m }
        );

        await context.SaveChangesAsync();

        return context;
    }
    #endregion

    #region AddAsync
    [Fact]
    public async Task ProductRepository_AddAsync_ReturnServiceResponseWithTrueAndProductDto()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new ProductRepository(context);
        var product = new ProductDto() { Name = "test", Description = "test" };
        var expected = true;

        // Act
        var result = await sut.AddAsync(product);
        await context.SaveChangesAsync();
        var secondResult = await context.Product.FirstOrDefaultAsync(x => x.Name.Equals("test"));

        // Assert
        Assert.IsType<ServiceResponse<ProductDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);

        Assert.NotNull(secondResult);
        Assert.Equal(result.Data.Id, secondResult.Id);
    }
    #endregion

    #region GetByIdAsync
    [Fact]
    public async Task ProductRepository_GetByIdAsync_ProductIdNotFoundReturnsSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new ProductRepository(context);
        var guid = Guid.NewGuid();
        var expected = false;


        // Act
        var result = await sut.GetByIdAsync(guid);

        // Assert
        Assert.IsType<ServiceResponse<ProductDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task ProductRepository_GetByIdAsync_ProductIdFoundReturnsSrWithTrue()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new ProductRepository(context);
        var guid = await context.Product.Select(x => x.Id).FirstOrDefaultAsync();
        var expected = true;


        // Act
        var result = await sut.GetByIdAsync(guid);

        // Assert
        Assert.IsType<ServiceResponse<ProductDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region GetAllAsync
    [Fact]
    public async Task ProductRepository_GetAllAsync_ReturnSrWithIEnumerableOfProductDtoWithTrue()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new ProductRepository(context);
        var expected = true;

        // Act
        var result = await sut.GetAllAsync();

        // Assert
        Assert.IsType<ServiceResponse<IEnumerable<ProductDto>>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region UpdateAsync
    [Fact]
    public async Task ProductRepository_UpdateAsync_ShouldNotFindProductToUpdateReturnSrWithFalse()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new ProductRepository(context);
        var expected = false;
        var product = new ProductDto() { Id = Guid.NewGuid() };

        // Act
        var result = await sut.UpdateAsync(product);

        // Assert
        Assert.IsType<ServiceResponse<ProductDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task ProductRepository_UpdateAsync_ReturnSrWithUpdatedProduct()
    {
        // Arrange
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new ProductRepository(context);
        var expected = true;
        var model = await context.Product.FirstAsync();
        var productDto = new ProductDto() { Id = model.Id, Name = "test" };


        // Act
        var result = await sut.UpdateAsync(productDto);
        await context.SaveChangesAsync();
        var secondResult = await context.Product.FindAsync(model.Id);


        // Assert
        Assert.NotNull(secondResult);
        Assert.Equal(secondResult.Name, productDto.Name);

        Assert.IsType<ServiceResponse<ProductDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);
    }
    #endregion

    #region RemoveAsync
    [Fact]
    public async Task ProductRepository_RemoveAsync_ShouldNotFindProductToRemoveReturnSrWithFalse()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new ProductRepository(context);
        var expected = false;
        var id = Guid.NewGuid();

        // Act
        var result = await sut.RemoveAsync(id);

        // Arrange
        Assert.IsType<ServiceResponse<ProductDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task ProductRepository_RemoveAsync_ReturnSrWithTrueAndRemovedProduct()
    {
        // Assert
        var context = await CreateShopContextWithInMemoryDbAsync();
        var sut = new ProductRepository(context);
        var expected = true;
        var model = await context.Product.FirstAsync();

        // Act
        var result = await sut.RemoveAsync(model.Id);
        await context.SaveChangesAsync();
        var secondResult = await context.Product.FindAsync(model.Id);

        // Arrange
        Assert.NotNull(model);

        Assert.IsType<ServiceResponse<ProductDto>>(result);
        Assert.Equal(expected, result.Success);
        Assert.NotNull(result.Data);

        Assert.Null(secondResult);
    }
    #endregion
}