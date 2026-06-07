using Cartridge.Data;
using Cartridge.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cartridge.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameRepository _gameRepository;

        public GamesController(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // GET /Games
        public async Task<IActionResult> Index()
        {
            var games = await _gameRepository.GetAllGames();
            return View(games);
        }
        // GET /Games/Details/5
        [HttpGet("/games/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameRepository.GetGameByID(id);
            if (game == null) return NotFound();
            return View(game);
        }


      


    }
}