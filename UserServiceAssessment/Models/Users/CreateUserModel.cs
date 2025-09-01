using System.ComponentModel.DataAnnotations;

namespace UserServiceAssessment.Models.User;

public class CreateUserModel
{
    [Required]
    public required string Name { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}