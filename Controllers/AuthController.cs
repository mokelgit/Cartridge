using Cartridge.Data;
using Cartridge.Models;
using Cartridge.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cartridge.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserRepository _userRepo;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("/auth/login")]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _authService.Login(model.Username, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/auth/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        [HttpGet("/auth/register")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }


        [HttpPost("/auth/register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState invalid:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($" - {error.ErrorMessage}");
                }
                return View(model);
            }

            
            await _authService.Register(model.Username, model.Password, model.Email);
            return RedirectToAction("Index", "Home");
        }
    }
}