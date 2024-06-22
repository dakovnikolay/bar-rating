using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BarRating.Models.ViewModels;

public class BarViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Bar name is required")]
    [StringLength(64, ErrorMessage = "Bar name must not exceed 64 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(255, ErrorMessage = "Description must not exceed 255 characters")]
    public string Description { get; set; }

    public string Image { get; set; }

    public IFormFile ImageFile { get; set; }
}

