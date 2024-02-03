using FakeItEasy;
using FastEndpoints;
using Server.Endpoints.Product.Delete;
using Server.Endpoints.Product.Get.GetAllProducts;
using Server.Endpoints.Product.Get.GetProductById;
using Server.Endpoints.Product.Post;
using Server.Endpoints.Product.Put;
using Shared;
using Shared.DataTransferObjects;
using Shared.Interfaces;

namespace Labb1_CleanCode_Solid.Server.Tests;

public class ProductEndpoints_Tests
{
    #region Add

    [Fact]
    public async Task ProductEndpoint_Add_Return200WithAddedProductDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var name = "Produkt 1";
        var req = new AddProductRequest
        {
            ProductDto = new ProductDto()
            {
                Name = name,
                UnitPrice = 50.33M
            }
        };
        var ep = Factory.Create<AddProductEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);
        A.CallTo(() => unitOfWork.ProductRepository.AddAsync(req.ProductDto)).Returns(new ServiceResponse<ProductDto>(true, req.ProductDto, ""));
        var expectedStatusCode = 200;

        // Act

        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ProductDto);
        Assert.IsAssignableFrom<ProductDto>(result.ProductDto);
        Assert.Equal(name, result.ProductDto.Name);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task ProductEndpoint_Add_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var name = "Produkt 1";
        var req = new AddProductRequest
        {
            ProductDto = new ProductDto()
            {
                Name = name,
                UnitPrice = 50.33M
            }
        };
        var ep = Factory.Create<AddProductEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);
        A.CallTo(() => unitOfWork.ProductRepository.AddAsync(req.ProductDto)).Returns(new ServiceResponse<ProductDto>(false, null, ""));
        var expectedStatusCode = 400;

        // Act

        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ProductDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region GetById

    [Fact]
    public async Task ProductEndpoint_GetById_Return200WithProductDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetProductByIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetProductByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.ProductRepository.GetByIdAsync(guid))
            .Returns(new ServiceResponse<ProductDto>(true, new ProductDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ProductDto);
        Assert.IsAssignableFrom<ProductDto>(result.ProductDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task ProductEndpoint_GetById_Return400FailGuidParse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var notAGuid = "not a guid";
        var req = new GetProductByIdRequest
        {
            Id = notAGuid
        };

        var ep = Factory.Create<GetProductByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ProductDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task ProductEndpoint_GetById_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetProductByIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetProductByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.ProductRepository.GetByIdAsync(guid))
            .Returns(new ServiceResponse<ProductDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }
    #endregion

    #region GetAllProducts

    [Fact]
    public async Task ProductEndpoint_GetAllProducts_Return200WithIEnumerableOfProductDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllProductsEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.ProductRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<ProductDto>>(true, new List<ProductDto>(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Products);
        Assert.IsAssignableFrom<IEnumerable<ProductDto>>(result.Products);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task ProductEndpoint_GetAllProducts_Return400FalseServiceResponse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllProductsEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.ProductRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<ProductDto>>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task ProductEndpoint_GetAllProducts_Return204TrueServiceResponseWithNoData()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllProductsEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.ProductRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<ProductDto>>(true, null, ""));

        var expectedStatusCode = 204;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region Update

    [Fact]
    public async Task ProductEndpoint_Update_Return200WithProductDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new UpdateProductRequest()
        {
            ProductDto = new ProductDto()
        };

        var ep = Factory.Create<UpdateProductEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.ProductRepository.UpdateAsync(req.ProductDto))
            .Returns(new ServiceResponse<ProductDto>(true, new ProductDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ProductDto);
        Assert.IsAssignableFrom<ProductDto>(result.ProductDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }
    
    [Fact]
    public async Task ProductEndpoint_Update_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new UpdateProductRequest()
        {
            ProductDto = new ProductDto()
        };

        var ep = Factory.Create<UpdateProductEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.ProductRepository.UpdateAsync(req.ProductDto))
            .Returns(new ServiceResponse<ProductDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ProductDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task ProductEndpoint_Delete_Return200WithProductDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new DeleteProductByIdRequest()
        {
            Id = $"{guid}"
        };

        var ep = Factory.Create<DeleteProductByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.ProductRepository.RemoveAsync(guid))
            .Returns(new ServiceResponse<ProductDto>(true, new ProductDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ProductDto);
        Assert.IsAssignableFrom<ProductDto>(result.ProductDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }
    
    [Fact]
    public async Task ProductEndpoint_Delete_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new DeleteProductByIdRequest()
        {
            Id = $"{guid}"
        };

        var ep = Factory.Create<DeleteProductByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.ProductRepository.RemoveAsync(guid))
            .Returns(new ServiceResponse<ProductDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ProductDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion
}