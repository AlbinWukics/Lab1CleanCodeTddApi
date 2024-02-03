using System.ComponentModel.DataAnnotations;

namespace Labb1_CleanCode_Solid.DataAccess.DataModels;

public class CustomerModel
{
    public Guid Id { get; set; }


    [Required, EmailAddress]
    public string Email { get; set; } = null!;


    public string Name { get; set; } = string.Empty;


    [Required]
    public string Password { get; set; } = null!;


    public DateTime CreatedDate { get; set; }


    public DateTime UpdatedDate { get; set; }
}