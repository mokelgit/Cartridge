using Cartridge.Data;
using Cartridge.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Security.Claims;

namespace Cartridge.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameRepository _gameRepository;



        public GamesController(GameRepository gameRepository)
        { 
            _gameRepository = gameRepository;
        }


        public async Task<IActionResult> Index(string? search, int? page)
        {
            IEnumerable<GameViewModel> games;

            if (!string.IsNullOrWhiteSpace(search))
                games = await _gameRepository.SearchGames(search);
            else
                games = await _gameRepository.GetAllGames(page ?? 0);

            ViewData["search"] = search;
            return View(games);
        }

        [HttpGet("/games/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameRepository.GetGameByID(id);
            if (game == null) return NotFound();
            //Set data for publishers developers and reviews
            var publishers = await _gameRepository.GetPublishersByGameId(game.ID);
            var developer = await _gameRepository.GetDevelopersByGameId(game.ID);
            var ratings = await _gameRepository.GetReviewsByGameID(game.ID);
            var reviews = await _gameRepository.GetReviewsWithBodyByGameId(game.ID);
            var platforms = await _gameRepository.GetPlatformsByGameId(game.ID);
            var reviewMeta = await _gameRepository.GetReviewMetaByGameID(game.ID);
            

            game.Publisher = publishers.ToList();
            game.Developer = developer.ToList();
            game.Ratings = ratings.ToList();
            game.Platforms = platforms.ToList();
            game.Reviews = reviews.ToList();
            game.ReviewMeta = reviewMeta;
            

            
            return View(game);
        }


        [HttpPost("/games/{id}")]
        public async Task<IActionResult> InsertNewReview(int id, DateTime reviewTime, string userID, string? reviewBody, int rating)
        {
            
            await _gameRepository.InsertReview(
                new Reviews
                {
                    GameID = id,
                    UserID = User.FindFirstValue("id"),
                    Rating = rating,
                    ReviewBody = reviewBody,
                    ReviewDate = DateTime.UtcNow,
                    Username = User.Identity.Name,
                }
            );
            return Redirect($"/games/{id}");
        }




    }
}