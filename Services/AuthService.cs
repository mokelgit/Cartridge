

using Cartridge.Data;
using BC = BCrypt.Net.BCrypt;
using Cartridge.Models;

namespace Cartridge.Services
{
    // Services/AuthService.cs — hashing lives here
    public class AuthService
    {
        private readonly UserRepository _userRepo;

        public AuthService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task Register(string username, string plainTextPassword, string email)
        {
            string hash = BC.HashPassword(plainTextPassword, workFactor: 12);
         
            await _userRepo.CreateUser(username, hash, email);
        }

        public async Task<User?> Login(string username, string plainTextPassword)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null) return null;
            if (!BC.Verify(plainTextPassword, user.PasswordHash)) return null;
            return user;
        }

        // In AuthService
        public async Task UpdatePassword(string id, string plainTextPassword)
        {
            string hash = BC.HashPassword(plainTextPassword, workFactor: 12);
            await _userRepo.UpdatePassword(id, hash);
        }
    }
} 
