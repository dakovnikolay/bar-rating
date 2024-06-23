using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BarRating.Services.Interfaces;
using BarRating.Models.ViewModels;
using BarRating.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BarRating.Data.Entities;

namespace BarRating.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IUserService _userService;
    private readonly IBarService _barService;
    private readonly IReviewService _reviewService;

    public AdminController(IWebHostEnvironment hostingEnvironment, IUserService userService, IBarService barService, IReviewService reviewService)
    {
        _hostingEnvironment = hostingEnvironment;
        _userService = userService;
        _barService = barService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var adminViewModel = await GetAdminViewModel();
        return View(adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.RegisterUserAsync(new RegisterViewModel
            {
                Username = model.Username,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName
            });
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if password is provided
            if (string.IsNullOrEmpty(model.Password))
            {
                var existingUser = await _userService.GetUserByIdAsync(model.Id);
                model.Password = existingUser.PasswordHash; 
            }

            var result = await _userService.UpdateUserAsync(model);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Admin");
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBar(BarViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                    // Check if the directory exists, if not, create it
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    // Generate a unique file name to avoid conflicts
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var imagePath = Path.Combine(imagesPath, uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    model.Image = $"/images/{uniqueFileName}"; // Save the URL relative to the web root
                }
                else
                {
                    model.Image = "";
                }

                var bar = new Bar
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = model.Image
                };

                await _barService.CreateBarAsync(bar);
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while uploading the image. Please try again.");
            }
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBar(BarViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var existingBar = await _barService.GetBarByIdAsync(model.Id);
                if (existingBar == null)
                {
                    return NotFound();
                }

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                    // Check if the directory exists, if not, create it
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    // Generate a unique file name to avoid conflicts
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var imagePath = Path.Combine(imagesPath, uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    existingBar.Image = $"/images/{uniqueFileName}"; // Save the URL relative to the web root
                }
                else if (string.IsNullOrEmpty(existingBar.Image) && string.IsNullOrEmpty(model.Image))
                {
                    existingBar.Image = "";
                }

                existingBar.Name = model.Name;
                existingBar.Description = model.Description;

                await _barService.UpdateBarAsync(existingBar);
                return RedirectToAction("Index", "Admin");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while uploading the image. Please try again.");
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBar(int barId)
    {
        var result = await _barService.DeleteBarAsync(barId);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Admin");
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    private async Task<AdminViewModel> GetAdminViewModel()
    {
        return new AdminViewModel
        {
            Bars = await _barService.GetAllBarsAsync(),
            Users = await _userService.GetAllUsersAsync(),
            UserCount = await _userService.GetUserCountAsync(),
            BarCount = await _barService.GetBarCountAsync(),
            ReviewCount = await _reviewService.GetReviewCountAsync()
        };
    }
}