using Cartridge.Data;
using Cartridge.Models;
using Cartridge.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Cartridge.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserRepository _userRepository;

        public AccountController(AuthService authService, UserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpGet("/account")]
        public async Task<IActionResult> Account()
        {
            var userId = User.FindFirst("id")?.Value;
            Console.WriteLine($"userId claim: {userId}");  // debug

            if (userId == null)
            {
                Console.WriteLine("No user ID claim found — not logged in?");
                return RedirectToAction("Login", "Auth");
            }

            var user = await _userRepository.GetUserByID(userId);
            Console.WriteLine($"user from DB: {user?.Username}");  // debug

            if (user == null)
            {
                Console.WriteLine("User not found in database");
                return RedirectToAction("Login", "Auth");
            }

            var model = new User
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Email = user.Email,
                ImageURL = user.ImageURL,
                Bio = user.Bio ?? "No bio."
            };

            return View(model);
        }
        // Update PFP on account page
        [HttpPost("/account")]
        public async Task<IActionResult> UpdateProfilePicture(string pfpURL) 
        {
            var userId = User.FindFirst("id")!.Value;
            await _userRepository.UpdateProfilePicture(userId, pfpURL);
            return RedirectToAction("Account");
        }

    }
}
