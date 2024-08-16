﻿using System.ComponentModel.DataAnnotations;

namespace Hermes.Application.Entities;
public class User
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"(?=.*\d)(?=.*[A-Z]).{8,}",
    ErrorMessage = "Password must be at least 8 characters long, contain at least one number and one uppercase letter.")]
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "collaborator";
}
