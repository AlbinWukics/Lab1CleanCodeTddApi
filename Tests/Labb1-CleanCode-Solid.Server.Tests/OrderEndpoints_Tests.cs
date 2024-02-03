using FakeItEasy;
using FastEndpoints;
using Server.Endpoints.Order.Delete;
using Server.Endpoints.Order.Get.GetAllOrders;
using Server.Endpoints.Order.Get.GetOrderById;
using Server.Endpoints.Order.Get.GetOrdersByCustomerId;
using Server.Endpoints.Order.Post;
using Server.Endpoints.Order.Put;
using Shared.DataTransferObjects;
using Shared.Interfaces;
using Shared;

namespace Labb1_CleanCode_Solid.Server.Tests;

public class OrderEndpoints_Tests
{
    #region Add

    [Fact]
    public async Task OrderEndpoint_Add_Return200WithAddedOrderDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new AddOrderRequest
        {
            OrderDto = new OrderDto()
            {
            }
        };

        var ep = Factory.Create<AddOrderEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.AddAsync(req.OrderDto)).Returns(new ServiceResponse<OrderDto>(true, req.OrderDto, ""));

        var expectedStatusCode = 200;

        // Act

        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.OrderDto);
        Assert.IsAssignableFrom<OrderDto>(result.OrderDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task OrderEndpoint_Add_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new AddOrderRequest
        {
            OrderDto = new OrderDto()
            {
            }
        };
        var ep = Factory.Create<AddOrderEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.AddAsync(req.OrderDto)).Returns(new ServiceResponse<OrderDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act

        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.OrderDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region GetById

    [Fact]
    public async Task OrderEndpoint_GetById_Return200WithOrderDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetOrderByIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetOrderByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.GetByIdAsync(guid))
            .Returns(new ServiceResponse<OrderDto>(true, new OrderDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.OrderDto);
        Assert.IsAssignableFrom<OrderDto>(result.OrderDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task OrderEndpoint_GetById_Return400FailGuidParse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var notAGuid = "not a guid";
        var req = new GetOrderByIdRequest
        {
            Id = notAGuid
        };

        var ep = Factory.Create<GetOrderByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.OrderDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task OrderEndpoint_GetById_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetOrderByIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetOrderByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.GetByIdAsync(guid))
            .Returns(new ServiceResponse<OrderDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }
    #endregion

    #region GetAllOrders

    [Fact]
    public async Task OrderEndpoint_GetAllOrders_Return200WithIEnumerableOfOrderDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllOrdersEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<OrderDto>>(true, new List<OrderDto>(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Orders);
        Assert.IsAssignableFrom<IEnumerable<OrderDto>>(result.Orders);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task OrderEndpoint_GetAllOrders_Return400FalseServiceResponse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllOrdersEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<OrderDto>>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task OrderEndpoint_GetAllOrders_Return204TrueServiceResponseWithNoData()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllOrdersEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<OrderDto>>(true, null, ""));

        var expectedStatusCode = 204;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region GetOrdersByCustomerId

    [Fact]
    public async Task OrderEndpoint_GetOrdersByCustomerId_Return200WithOrderDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetOrdersByCustomerIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetOrdersByCustomerIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.GetByCustomerIdAsync(guid))
            .Returns(new ServiceResponse<IEnumerable<OrderDto>>(true, new List<OrderDto>(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Orders);
        Assert.IsAssignableFrom<IEnumerable<OrderDto>>(result.Orders);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task OrderEndpoint_GetOrdersByCustomerId_Return400FailGuidParse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var notAGuid = "not a guid";
        var req = new GetOrdersByCustomerIdRequest
        {
            Id = notAGuid
        };

        var ep = Factory.Create<GetOrdersByCustomerIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Orders);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task OrderEndpoint_GetOrdersByCustomerId_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetOrdersByCustomerIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetOrdersByCustomerIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.GetByCustomerIdAsync(guid))
            .Returns(new ServiceResponse<IEnumerable<OrderDto>>(false, null, ""));

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
    public async Task OrderEndpoint_Update_Return200WithOrderDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new UpdateOrderRequest()
        {
            OrderDto = new OrderDto()
        };

        var ep = Factory.Create<UpdateOrderEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.UpdateAsync(req.OrderDto))
            .Returns(new ServiceResponse<OrderDto>(true, new OrderDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.OrderDto);
        Assert.IsAssignableFrom<OrderDto>(result.OrderDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task OrderEndpoint_Update_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new UpdateOrderRequest()
        {
            OrderDto = new OrderDto()
        };

        var ep = Factory.Create<UpdateOrderEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.UpdateAsync(req.OrderDto))
            .Returns(new ServiceResponse<OrderDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.OrderDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task OrderEndpoint_Delete_Return200WithOrderDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new DeleteOrderByIdRequest()
        {
            Id = $"{guid}"
        };

        var ep = Factory.Create<DeleteOrderByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.RemoveAsync(guid))
            .Returns(new ServiceResponse<OrderDto>(true, new OrderDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.OrderDto);
        Assert.IsAssignableFrom<OrderDto>(result.OrderDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task OrderEndpoint_Delete_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new DeleteOrderByIdRequest()
        {
            Id = $"{guid}"
        };

        var ep = Factory.Create<DeleteOrderByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.OrderRepository.RemoveAsync(guid))
            .Returns(new ServiceResponse<OrderDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.OrderDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion
}