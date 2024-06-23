using BarRating.Data.Entities;

namespace BarRating.Services.Interfaces;

public interface IReviewService
{
    Task<List<Review>> GetReviewsByBarIdAsync(int barId);
    Task<Review> GetReviewByIdAsync(int reviewId);
    Task<Review> GetUserReviewByBarIdAsync(string userId, int barId);
    Task CreateReviewAsync(Review review);
    Task UpdateReviewAsync(Review review);
    Task DeleteReviewAsync(int reviewId);
    Task<int> GetReviewCountAsync();
}