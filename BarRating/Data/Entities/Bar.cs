namespace BarRating.Data.Entities;

public class Bar
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }
}
