using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cartridge.Models;
using Cartridge.Data;

namespace Cartridge.Controllers;

public class HomeController : Controller
{
    private readonly GameRepository _gameRepository;

    public HomeController(GameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<IActionResult> Index()
    {
        var randomGames = await _gameRepository.GetRandomGames();
        return View(randomGames);
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
