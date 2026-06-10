using Cartridge.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cartridge.Controllers
{
    public class PlatformController : Controller
    {
        private readonly PlatformRepository _platformRepository;

        public PlatformController(PlatformRepository platformRepository)
        {
            _platformRepository = platformRepository;
        }
        public async Task<IActionResult> Index()
        {
            var platforms = await _platformRepository.GetAllPlatforms();
            return View(platforms);
        }

        [HttpGet("/platforms/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var platforms = await _platformRepository.GetPlatformByID(id);
            if (platforms == null) return NotFound();
            return View(platforms);
        }
    }
}
