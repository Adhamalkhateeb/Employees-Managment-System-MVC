using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Identity;

public sealed class RegisterRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(
        50,
        MinimumLength = 3,
        ErrorMessage = "Username must be between 3 and 50 characters"
    )]
    [RegularExpression(
        @"^[a-zA-Z0-9_]+$",
        ErrorMessage = "Username can only contain letters, numbers, and underscores"
    )]
    [Display(Name = "Username")]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [Display(Name = "Email address")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format")]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = default!;
}
