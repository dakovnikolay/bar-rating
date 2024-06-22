using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BarRating.Models;
using BarRating.Services.Interfaces;
using BarRating.Data.ViewModels;
using BarRating.Data.Entities;
using System.Security.Claims;

namespace BarRating.Controllers;

public class BarController : Controller
{
    private readonly IBarService _barService;
    private readonly IReviewService _reviewService;

    public BarController(IBarService barService, IReviewService reviewService)
    {
        _barService = barService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var bar = await _barService.GetBarByIdAsync(id);
        var reviews = await _reviewService.GetReviewsByBarIdAsync(id);
        var viewModel = new BarDetailsViewModel
        {
            Bar = bar,
            Reviews = reviews,
            UserHasReviewed = !reviews.Any(r => r.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)) // Assuming user ID from claims
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddReview(Review review)
    {
        review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims
        await _reviewService.CreateReviewAsync(review);
        return RedirectToAction("Details", new { id = review.BarId });
    }

    [HttpPost]
    public async Task<IActionResult> EditReview(Review review)
    {
        await _reviewService.UpdateReviewAsync(review);
        return RedirectToAction("Details", new { id = review.BarId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteReview(int reviewId, int barId)
    {
        await _reviewService.DeleteReviewAsync(reviewId);
        return RedirectToAction("Details", new { id = barId });
    }

}
