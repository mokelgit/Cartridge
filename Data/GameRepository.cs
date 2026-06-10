using Cartridge.Models;
using Dapper;
using MySqlConnector;
using System.Data;

namespace Cartridge.Data
{
    public class GameRepository 
    {

        private readonly IDbConnection _db;
        private readonly string _connectionString;
        public GameRepository(IDbConnection db, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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
            slug AS Slug
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
            slug AS Slug
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
            slug as Slug
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
            slug as Slug
            FROM games
            WHERE id = @id",
                new { id }
            );
        }

        public async Task<IEnumerable<Companies>> GetCompaniesByGameId(int id)
        {
            const string query = @"
            SELECT c.id, c.name, gc.is_developer, gc.is_publisher
            FROM companies c
            JOIN game_companies gc ON c.id = gc.company_id
            WHERE gc.game_id = @GameID";

            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Companies>(query, new { GameID = id });
        }

        public async Task<IEnumerable<Companies>> GetPublishersByGameId(int id)
        {
            const string query = @"
            SELECT c.id, c.name, gc.is_developer, gc.is_publisher
            FROM companies c
            JOIN game_companies gc ON c.id = gc.company_id
            WHERE gc.game_id = @GameID AND gc.is_publisher = true";

            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Companies>(query, new { GameID = id });
        }

        public async Task<IEnumerable<Companies>> GetDevelopersByGameId(int id)
        {
            const string query = @"
            SELECT c.id, c.name, gc.is_developer, gc.is_publisher
            FROM companies c
            JOIN game_companies gc ON c.id = gc.company_id
            WHERE gc.game_id = @GameID AND gc.is_developer = true";

            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Companies>(query, new { GameID = id });
        }


        public async Task<IEnumerable<GameViewModel>> SearchGames(string search)
        {
            const string sql = @"
            SELECT 
                id AS ID,
                game_name AS Name,
                release_date AS ReleaseDate,
                background_image_url AS BackgroundImageURL,
                cover_image_url AS CoverImageURL,
                slug AS Slug
            FROM games
            WHERE game_name LIKE @Search
            LIMIT 40";

            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<GameViewModel>(sql, new { Search = $"%{search}%" });
        }




    }
}
