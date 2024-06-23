using BarRating.Data;
using BarRating.Data.Entities;
using BarRating.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarRating.Services;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetReviewsByBarIdAsync(int barId)
    {
        return await _context.Reviews
            .Where(r => r.BarId == barId)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<Review> GetReviewByIdAsync(int reviewId)
    {
        return await _context.Reviews.FindAsync(reviewId);
    }

    public async Task<Review> GetUserReviewByBarIdAsync(string userId, int barId)
    {
        return await _context.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.BarId == barId);
    }

    public async Task CreateReviewAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateReviewAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(int reviewId)
    {
        var review = await GetReviewByIdAsync(reviewId);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetReviewCountAsync()
    {
        return await _context.Reviews.CountAsync();
    }
}