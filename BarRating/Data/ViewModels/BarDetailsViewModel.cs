using BarRating.Data.Entities;

namespace BarRating.Data.ViewModels;

public class BarDetailsViewModel
{
    public Bar Bar { get; set; }
    public List<Review> Reviews { get; set; }
    public bool UserHasReviewed { get; set; }
}
