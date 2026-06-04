using Cartridge.Models;
using Dapper;
using System.Data;

namespace Cartridge.Data
{
    public class GameRepository
    {

        private readonly IDbConnection _db;

        public GameRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GameViewModel>> GetAllGames()
        {
            return await _db.QueryAsync<GameViewModel>(
                @"SELECT 
            id AS ID,
            game_name AS Name,
            release_date AS ReleaseDate,
            background_image_url AS BackgroundImageURL,
            cover_image_url AS CoverImageURL,
            slug AS Slug,
            publisher AS Publisher,
            developer AS Developer
          FROM games
          ORDER BY game_name ASC"
            );
        }

        public async Task<IEnumerable<GameViewModel>> GetRandomGames(int count = 6)
        {
            return await _db.QueryAsync<GameViewModel>(
                @"SELECT 
            id AS ID,
            game_name AS Name,
            release_date AS ReleaseDate,
            background_image_url AS BackgroundImageURL,
            cover_image_url AS CoverImageURL,
            slug AS Slug,
            publisher AS Publisher,
            developer AS Developer
          FROM games
          WHERE cover_image_url IS NOT NULL
          ORDER BY RAND()
          LIMIT @count",
                new { count }
            );
        }
        public async Task<GameViewModel?> GetGameByName(string name)
        {
            return await _db.QuerySingleOrDefaultAsync<GameViewModel>(
                @"SELECT 
            id AS ID,
            game_name AS Name,
            release_date AS ReleaseDate,
            background_image_url AS BackgroundImageURL,
            cover_image_url AS CoverImageURL,
            slug as Slug,
            publisher as Publisher,
            developer as Developer
            FROM games
            WHERE game_name = @name",
                new { name }
            );
        }
        public async Task<GameViewModel?> GetGameByID(int id)
        {
            return await _db.QuerySingleOrDefaultAsync<GameViewModel>(
                @"SELECT 
            id AS ID,
            game_name AS Name,
            release_date AS ReleaseDate,
            background_image_url AS BackgroundImageURL,
            cover_image_url AS CoverImageURL,
            slug as Slug,
            publisher as Publisher,
            developer as Developer
            FROM games
            WHERE id = @id",
                new { id }
            );
        }
    }
}
