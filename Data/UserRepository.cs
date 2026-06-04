using Cartridge.Models;
using Dapper;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Cartridge.Data
{
    public class UserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task CreateUser(string username, string passwordHash, string email)
        {
            await _db.ExecuteAsync(
                "INSERT INTO users (username, password_hash, email) VALUES (@username, @passwordHash, @email)",
                new { username, passwordHash, email }
            );

        }

        // Get user by ID — useful for loading the account page
        public async Task<User?> GetUserByID(string id)
        {
            return await _db.QuerySingleOrDefaultAsync<User>(
                @"SELECT 
                    id AS Id,
                    username AS Username,
                    password_hash AS PasswordHash,
                    email AS Email,
                    created_at AS CreatedAt,
                    profile_picture_url AS ProfilePictureURL
                  FROM users 
                  WHERE id = @id",
                new { id }
            );
        }

        // Update username
        public async Task UpdateUsername(string id, string newUsername)
        {
            await _db.ExecuteAsync(
                "UPDATE users SET username = @newUsername WHERE id = @id",
                new { id, newUsername }
            );
        }

        // Update email
        public async Task UpdateEmail(string id, string newEmail)
        {
            await _db.ExecuteAsync(
                "UPDATE users SET email = @newEmail WHERE id = @id",
                new { id, newEmail }
            );
        }

        // Update password
        public async Task UpdatePassword(string id, string newPasswordHash)
        {
            await _db.ExecuteAsync(
                "UPDATE users SET password_hash = @newPasswordHash WHERE id = @id",
                new { id, newPasswordHash }
            );
        }

        // Update profile picture
        public async Task UpdateProfilePicture(string id, string imageURL)
        {
            await _db.ExecuteAsync(
                "UPDATE users SET profile_picture_url = @imageURL WHERE id = @id",
                new { id, imageURL }
            );
        }

        public async Task UpdateBio(string id, string bio)
        {
            var validLength = bio.Length < 300;
            if (validLength)
            {
                await _db.ExecuteAsync(
                    "UPDATE users SET bio = @bio WHERE id = @id",
                    new { id, bio }
);
            }else
            {
                Console.WriteLine("failed to update");
            }
        }
        public async Task<User?> GetUserByUsername(string username)
        {
            return await _db.QuerySingleOrDefaultAsync<User>(
                @"SELECT 
            id AS Id,
            username AS Username,
            password_hash AS PasswordHash,
            email AS Email,
            created_at AS CreatedAt
            FROM users 
            WHERE username = @username",
                new { username }
            );
        }
    }
}
