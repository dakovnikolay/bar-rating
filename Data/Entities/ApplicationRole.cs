using Microsoft.AspNetCore.Identity;

namespace BarRating.Data.Entities;

public class ApplicationRole : IdentityRole
{
    public string Description { get; set; }
}
