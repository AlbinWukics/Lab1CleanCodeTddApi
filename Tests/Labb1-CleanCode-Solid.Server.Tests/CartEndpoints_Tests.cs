using FakeItEasy;
using FastEndpoints;
using Server.Endpoints.Cart.Delete;
using Server.Endpoints.Cart.Get.GetAllCarts;
using Server.Endpoints.Cart.Get.GetCartById;
using Server.Endpoints.Cart.Get.GetCartsByCustomerId;
using Server.Endpoints.Cart.Post;
using Server.Endpoints.Cart.Put;
using Shared;
using Shared.DataTransferObjects;
using Shared.Interfaces;

namespace Labb1_CleanCode_Solid.Server.Tests;

public class CartEndpoints_Tests
{
    #region Add

    [Fact]
    public async Task CartEndpoint_Add_Return200WithAddedCartDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new AddCartRequest
        {
            CartDto = new CartDto()
            {
            }
        };

        var ep = Factory.Create<AddCartEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.AddAsync(req.CartDto)).Returns(new ServiceResponse<CartDto>(true, req.CartDto, ""));

        var expectedStatusCode = 200;

        // Act

        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CartDto);
        Assert.IsAssignableFrom<CartDto>(result.CartDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CartEndpoint_Add_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new AddCartRequest
        {
            CartDto = new CartDto()
            {
            }
        };
        var ep = Factory.Create<AddCartEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.AddAsync(req.CartDto)).Returns(new ServiceResponse<CartDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.CartDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region GetById

    [Fact]
    public async Task CartEndpoint_GetById_Return200WithCartDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetCartByIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetCartByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.GetByIdAsync(guid))
            .Returns(new ServiceResponse<CartDto>(true, new CartDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CartDto);
        Assert.IsAssignableFrom<CartDto>(result.CartDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CartEndpoint_GetById_Return400FailGuidParse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var notAGuid = "not a guid";
        var req = new GetCartByIdRequest
        {
            Id = notAGuid
        };

        var ep = Factory.Create<GetCartByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.CartDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CartEndpoint_GetById_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetCartByIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetCartByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.GetByIdAsync(guid))
            .Returns(new ServiceResponse<CartDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }
    #endregion

    #region GetAllCarts

    [Fact]
    public async Task CartEndpoint_GetAllCarts_Return200WithIEnumerableOfCartDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllCartsEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<CartDto>>(true, new List<CartDto>(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Carts);
        Assert.IsAssignableFrom<IEnumerable<CartDto>>(result.Carts);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CartEndpoint_GetAllCarts_Return400FalseServiceResponse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllCartsEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<CartDto>>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CartEndpoint_GetAllCarts_Return204TrueServiceResponseWithNoData()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllCartsEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<CartDto>>(true, null, ""));

        var expectedStatusCode = 204;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region GetCartsByCustomerId

    [Fact]
    public async Task CartEndpoint_GetCartsByCustomerId_Return200WithCartDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetCartsByCustomerIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetCartsByCustomerIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.GetByCustomerIdAsync(guid))
            .Returns(new ServiceResponse<IEnumerable<CartDto>>(true, new List<CartDto>(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Carts);
        Assert.IsAssignableFrom<IEnumerable<CartDto>>(result.Carts);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CartEndpoint_GetCartsByCustomerId_Return400FailGuidParse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var notAGuid = "not a guid";
        var req = new GetCartsByCustomerIdRequest
        {
            Id = notAGuid
        };

        var ep = Factory.Create<GetCartsByCustomerIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Carts);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CartEndpoint_GetCartsByCustomerId_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetCartsByCustomerIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetCartsByCustomerIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.GetByCustomerIdAsync(guid))
            .Returns(new ServiceResponse<IEnumerable<CartDto>>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region Update

    [Fact]
    public async Task CartEndpoint_Update_Return200WithCartDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new UpdateCartRequest()
        {
            CartDto = new CartDto()
        };

        var ep = Factory.Create<UpdateCartEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.UpdateAsync(req.CartDto))
            .Returns(new ServiceResponse<CartDto>(true, new CartDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CartDto);
        Assert.IsAssignableFrom<CartDto>(result.CartDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CartEndpoint_Update_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new UpdateCartRequest()
        {
            CartDto = new CartDto()
        };

        var ep = Factory.Create<UpdateCartEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.UpdateAsync(req.CartDto))
            .Returns(new ServiceResponse<CartDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.CartDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task CartEndpoint_Delete_Return200WithCartDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new DeleteCartByIdRequest()
        {
            Id = $"{guid}"
        };

        var ep = Factory.Create<DeleteCartByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.RemoveAsync(guid))
            .Returns(new ServiceResponse<CartDto>(true, new CartDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CartDto);
        Assert.IsAssignableFrom<CartDto>(result.CartDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CartEndpoint_Delete_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new DeleteCartByIdRequest()
        {
            Id = $"{guid}"
        };

        var ep = Factory.Create<DeleteCartByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CartRepository.RemoveAsync(guid))
            .Returns(new ServiceResponse<CartDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.CartDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion
}