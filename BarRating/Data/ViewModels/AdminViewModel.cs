using BarRating.Data.Entities;

namespace BarRating.Data.ViewModels;

public class AdminViewModel
{
    public List<Bar> Bars { get; set; }
    public List<ApplicationUser> Users { get; set; }
    public int UserCount { get; set; }
    public int BarCount { get; set; }
    public int ReviewCount { get; set; }
}
