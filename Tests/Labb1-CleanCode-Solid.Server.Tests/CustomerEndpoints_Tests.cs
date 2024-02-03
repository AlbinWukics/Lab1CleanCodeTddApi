using FakeItEasy;
using FastEndpoints;
using Server.Endpoints.Customer.Delete;
using Server.Endpoints.Customer.Get.GetAllCustomers;
using Server.Endpoints.Customer.Get.GetCustomerById;
using Server.Endpoints.Customer.Post;
using Server.Endpoints.Customer.Put;
using Shared.DataTransferObjects;
using Shared.Interfaces;
using Shared;

namespace Labb1_CleanCode_Solid.Server.Tests;

public class CustomerEndpoints_Tests
{
    #region Add

    [Fact]
    public async Task CustomerEndpoint_Add_Return200WithAddedCustomerDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var email = "mail@mail.mail";
        var req = new AddCustomerRequest
        {
            CustomerDto = new CustomerDto()
            {
                Email = email
            }
        };
        var ep = Factory.Create<AddCustomerEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.AddAsync(req.CustomerDto)).Returns(new ServiceResponse<CustomerDto>(true, req.CustomerDto, ""));
        
        var expectedStatusCode = 200;

        // Act

        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CustomerDto);
        Assert.IsAssignableFrom<CustomerDto>(result.CustomerDto);
        Assert.Equal(email, result.CustomerDto.Email);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CustomerEndpoint_Add_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var email = "mail@mail.mail";
        var req = new AddCustomerRequest
        {
            CustomerDto = new CustomerDto()
            {
                Email = email
            }
        };
        var ep = Factory.Create<AddCustomerEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.AddAsync(req.CustomerDto)).Returns(new ServiceResponse<CustomerDto>(false, null, ""));
        
        var expectedStatusCode = 400;

        // Act

        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.CustomerDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region GetById

    [Fact]
    public async Task CustomerEndpoint_GetById_Return200WithCustomerDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetCustomerByIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetCustomerByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.GetByIdAsync(guid))
            .Returns(new ServiceResponse<CustomerDto>(true, new CustomerDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CustomerDto);
        Assert.IsAssignableFrom<CustomerDto>(result.CustomerDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CustomerEndpoint_GetById_Return400FailGuidParse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var notAGuid = "not a guid";
        var req = new GetCustomerByIdRequest
        {
            Id = notAGuid
        };

        var ep = Factory.Create<GetCustomerByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.CustomerDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CustomerEndpoint_GetById_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new GetCustomerByIdRequest
        {
            Id = $"{guid}"
        };
        var ep = Factory.Create<GetCustomerByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.GetByIdAsync(guid))
            .Returns(new ServiceResponse<CustomerDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }
    #endregion

    #region GetAllCustomers

    [Fact]
    public async Task CustomerEndpoint_GetAllCustomers_Return200WithIEnumerableOfCustomerDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllCustomersEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<CustomerDto>>(true, new List<CustomerDto>(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Customers);
        Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(result.Customers);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CustomerEndpoint_GetAllCustomers_Return400FalseServiceResponse()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllCustomersEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<CustomerDto>>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CustomerEndpoint_GetAllCustomers_Return204TrueServiceResponseWithNoData()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();

        var ep = Factory.Create<GetAllCustomersEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.GetAllAsync())
            .Returns(new ServiceResponse<IEnumerable<CustomerDto>>(true, null, ""));

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
    public async Task CustomerEndpoint_Update_Return200WithCustomerDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new UpdateCustomerRequest()
        {
            CustomerDto = new CustomerDto()
        };

        var ep = Factory.Create<UpdateCustomerEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.UpdateAsync(req.CustomerDto))
            .Returns(new ServiceResponse<CustomerDto>(true, new CustomerDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CustomerDto);
        Assert.IsAssignableFrom<CustomerDto>(result.CustomerDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CustomerEndpoint_Update_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new UpdateCustomerRequest()
        {
            CustomerDto = new CustomerDto()
        };

        var ep = Factory.Create<UpdateCustomerEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.UpdateAsync(req.CustomerDto))
            .Returns(new ServiceResponse<CustomerDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.CustomerDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task CustomerEndpoint_Delete_Return200WithCustomerDto()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new DeleteCustomerByIdRequest()
        {
            Id = $"{guid}"
        };

        var ep = Factory.Create<DeleteCustomerByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.RemoveAsync(guid))
            .Returns(new ServiceResponse<CustomerDto>(true, new CustomerDto(), ""));

        var expectedStatusCode = 200;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.CustomerDto);
        Assert.IsAssignableFrom<CustomerDto>(result.CustomerDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task CustomerEndpoint_Delete_Return400()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var guid = Guid.NewGuid();
        var req = new DeleteCustomerByIdRequest()
        {
            Id = $"{guid}"
        };

        var ep = Factory.Create<DeleteCustomerByIdEndpoint>(ctx =>
            ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        A.CallTo(() => unitOfWork.CustomerRepository.RemoveAsync(guid))
            .Returns(new ServiceResponse<CustomerDto>(false, null, ""));

        var expectedStatusCode = 400;

        // Act
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.CustomerDto);
        Assert.Equal(expectedStatusCode, ep.HttpContext.Response.StatusCode);
    }

    #endregion
}