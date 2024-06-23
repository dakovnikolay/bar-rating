using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BarRating.Services.Interfaces;
using BarRating.Data.ViewModels;

namespace BarRating.Controllers;

public class HomeController : Controller
{

    private readonly IBarService _barService;

    public HomeController(IBarService barService)
    {
        _barService = barService;
    }


    public async Task<IActionResult> Index()
    {
        var bars = await _barService.GetAllBarsAsync();
        return View(bars);  // Sends the list of bars to the Home/Index view
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
