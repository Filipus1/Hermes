using System.ComponentModel.DataAnnotations;

namespace Hermes.Application.Entities.Dto;
public record UserDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters.")]
    [RegularExpression(@"(?=.*\d)(?=.*[A-Z]).{8,}", ErrorMessage = "Password must be at least 8 characters long, contain at least one number and one uppercase letter in Dto.")]
    public string Password { get; set; } = string.Empty;
}