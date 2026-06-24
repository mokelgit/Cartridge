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

        public async Task<IEnumerable<GameViewModel>> GetAllGames(int page = 0)
        {
            int pageSize = 40;
            int offset = page * pageSize;

            return await _db.QueryAsync<GameViewModel>(
                @"SELECT 
            id AS ID,
            game_name AS Name,
            release_date AS ReleaseDate,
            background_image_url AS BackgroundImageURL,
            cover_image_url AS CoverImageURL,
            slug AS Slug
            FROM games
            ORDER BY id ASC
            LIMIT @pageSize OFFSET @offset",
            new { pageSize = (long)pageSize, offset = (long)offset }
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

        public async Task<IEnumerable<GameViewModel>> GetGamesByIds(int[] ids)
        {
            return await _db.QueryAsync<GameViewModel>(
                @"SELECT 
        id AS ID,
        game_name AS Name,
        release_date AS ReleaseDate,
        background_image_url AS BackgroundImageURL,
        cover_image_url AS CoverImageURL,
        slug as Slug
        FROM games
        WHERE id IN @ids",
                new { ids }
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

        public async Task<IEnumerable<Reviews>> GetReviewsWithBodyByGameId(int gameID)
        {
            return await _db.QueryAsync<Reviews>(
                @"SELECT
            game_id AS GameID,
            users_id AS UserID,
            rating AS Rating,
            review_body AS ReviewBody,
            review_date AS ReviewDate,
            username AS Username
          FROM reviews
          WHERE review_body IS NOT NULL
          AND game_id = @gameID",
                new { gameID });
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

        public async Task<IEnumerable<Platforms>> GetPlatformsByGameId(int id) 
        {
            const string query = @"
            SELECT 
                p.platform_id AS PlatformID,
                p.platform_name AS PlatformName,
                p.slug AS Slug,
                p.icon AS Icon,
                p.short_name AS ShortName
            FROM platforms p
            JOIN game_platforms gp ON p.platform_id = gp.platform_id
            WHERE gp.game_id = @GameID";

            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Platforms>(query, new { GameID = id });

        }


        public async Task<GameReviewMeta> GetReviewMetaByGameID(int gameID)
        {
            return await _db.QuerySingleAsync<GameReviewMeta>(
                @"SELECT 
            COUNT(CASE WHEN rating IN (9, 10) THEN 1 END) AS FiveStars,
            COUNT(CASE WHEN rating IN (7, 8) THEN 1 END) AS FourStars,
            COUNT(CASE WHEN rating IN (5, 6) THEN 1 END) AS ThreeStars,
            COUNT(CASE WHEN rating IN (3, 4) THEN 1 END) AS TwoStars,
            COUNT(CASE WHEN rating IN (1, 2) THEN 1 END) AS OneStars,
            AVG(rating) AS AverageRating
          FROM reviews
          WHERE game_id = @gameID",
                new { gameID });
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

        public async Task<IEnumerable<Reviews>> GetReviewsByGameID(int id)
        {
            return await _db.QueryAsync<Reviews>
            (@"SELECT 
                game_id AS GameID,
                users_id AS UserID,
                rating AS Rating,
                review_body AS ReviewBody,
                review_date AS ReviewDate,
                username AS Username
            FROM reviews
            WHERE game_id = @id",
             new { id });

        }


        public async Task InsertReview(Reviews review)
        {
            var sql = @"INSERT INTO reviews (game_id, users_id, rating, review_body, review_date, username)
                                VALUES (@GameID, @UserID, @Rating, @ReviewBody, @ReviewDate, @Username)";

            await _db.ExecuteAsync(sql, review);
        }



    }
}
