using System.ComponentModel.DataAnnotations;

namespace BarRating.Data.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(64, ErrorMessage = "Username must be between 3 and 64 characters", MinimumLength = 3)]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(128, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [MaxLength(35)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(35)]
    public string LastName { get; set; }
}

