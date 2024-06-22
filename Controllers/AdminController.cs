using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BarRating.Services.Interfaces;
using BarRating.Models.ViewModels;
using BarRating.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BarRating.Data.Entities;

namespace BarRating.Controllers;

//[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IUserService _userService;
    private readonly IBarService _barService;

    public AdminController(IWebHostEnvironment hostingEnvironment, IUserService userService, IBarService barService)
    {
        _hostingEnvironment = hostingEnvironment;
        _userService = userService;
        _barService = barService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllUsersAsync();
        var bars = await _barService.GetAllBarsAsync();
        var viewModel = new AdminViewModel
        {
            Users = users,
            UserCount = users.Count, 
            Bars = bars,
            BarCount = bars.Count,
            ReviewCount = bars.Sum(b => b.Reviews?.Count ?? 0) 
        };

        return View(viewModel);
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
        return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
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
        return RedirectToAction("Index", "Admin");
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
        return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBar(Bar model, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", imageFile.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.Image = $"/images/{imageFile.FileName}"; // Save the URL relative to the web root
            }

            await _barService.CreateBarAsync(model);
            return RedirectToAction("Index", "Admin");
        }
        return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBar(Bar model)
    {
        if (ModelState.IsValid)
        {
            await _barService.UpdateBarAsync(model);
            return RedirectToAction("Index", "Admin");
        }
        return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBar(int id)
    {
        await _barService.DeleteBarAsync(id);
        return RedirectToAction("Index", "Admin");
    }
}
