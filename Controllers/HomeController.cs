using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cartridge.Models;
using Cartridge.Data;

namespace Cartridge.Controllers;

public class HomeController : Controller
{
    private readonly GameRepository _gameRepository;
    private static readonly int[] TOPPICKS = { 987, 823, 3979, 7, 73, 340 };
    public HomeController(GameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<IActionResult> Index()
    {
        var topGames = await _gameRepository.GetGamesByIds(TOPPICKS);
        return View(topGames);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
