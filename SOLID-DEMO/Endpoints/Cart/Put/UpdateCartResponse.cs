using Shared.DataTransferObjects;

namespace Server.Endpoints.Cart.Put;

public class UpdateCartResponse
{
    //public IResult Result { get; set; }

    public CartDto? CartDto { get; set; }
}