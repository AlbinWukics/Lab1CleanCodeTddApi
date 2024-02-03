using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public class CustomerDto
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