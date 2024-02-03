namespace Labb1_CleanCode_Solid.Shared.Tests;

public class Customer_Tests
{
    [Fact]
    public void Customer_PassEmailAddressValidation()
    {
        // Arrange
        var customer = new CustomerDto() { Email = "valid.email@example.com", Password = "password" };

        // Act
        var validationContext = new ValidationContext(customer);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(customer, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid, "Valid email should pass validation");
    }

    [Fact]
    public void Customer_FailEmailAddressValidation()
    {
        // Arrange
        var customer = new CustomerDto() { Email = "hej", Password = "hej" };

        // Act
        var validationContext = new ValidationContext(customer);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(customer, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid, "Invalid email should fail validation");
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Email"));
    }
}