using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace BarRating.Data.Entities;

public class Review
{
    public int Id { get; set; }

    [Required]
    public int Rating { get; set; }

    [Required]
    [StringLength(500)]
    public string Comment { get; set; }

    [Required]
    public string UserId { get; set; }

    [ValidateNever]
    public virtual ApplicationUser User { get; set; }

    [Required]
    public int BarId { get; set; }

    [ValidateNever]
    public virtual Bar Bar { get; set; }
}