using Cartridge.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cartridge.Controllers
{
    public class GameCompController : Controller
    {
            
        private readonly GameCompRepo _gameCompRepo;

        public GameCompController(GameCompRepo gameCompRepo)
        {
            _gameCompRepo = gameCompRepo;
        }
        public async Task<IActionResult> Index()
        {
            var gameCompanies = await _gameCompRepo.GetAllGameCompanies();
            return View(gameCompanies);
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var gameCompanies = await _gameCompRepo.GetByGameID(id);
            if (gameCompanies == null) return NotFound();
            return View(gameCompanies);
        }
    }
}
