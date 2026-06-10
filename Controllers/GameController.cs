using Cartridge.Data;
using Cartridge.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Cartridge.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameRepository _gameRepository;
        private readonly GameCompRepo _gameCompRepo;



        public GamesController(GameRepository gameRepository, GameCompRepo gameCompRepo)
        { 
            _gameRepository = gameRepository;
            _gameCompRepo = gameCompRepo;
        }


        public async Task<IActionResult> Index(string? search)
        {
            IEnumerable<GameViewModel> games;

            if (!string.IsNullOrWhiteSpace(search))
                games = await _gameRepository.SearchGames(search);
            else
                games = await _gameRepository.GetAllGames();

            ViewData["search"] = search;
            return View(games);
        }

        [HttpGet("/games/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameRepository.GetGameByID(id);
            if (game == null) return NotFound();

            var publishers = await _gameRepository.GetPublishersByGameId(game.ID);
            var developer = await _gameRepository.GetDevelopersByGameId(game.ID);

            game.Publisher = publishers.ToList();
            game.Developer = developer.ToList();
            
            return View(game);
        }




    }
}