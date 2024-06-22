namespace BarRating.Data.Entities;
public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }

    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    public int BarId { get; set; }
    public virtual Bar Bar { get; set; }
}
