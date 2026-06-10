using Cartridge.Models;
using Dapper;
using System.Data;

namespace Cartridge.Data
{
    public class GamePlatformRepo
    {
        private readonly IDbConnection _db;

        public GamePlatformRepo(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GamePlatforms>> GetAllGamePlatforms()
        {
            return await _db.QueryAsync<GamePlatforms>
                (@"SELECT
                game_id AS GameID,
                platform_id AS PlatformID
                FROM game_companies
                ");
        }

        public async Task<GamePlatforms?> GetByGameID(int id)
        {
            return await _db.QuerySingleOrDefaultAsync<GamePlatforms>
            (@" SELECT 
                game_id AS GameID,
                platform_id AS PlatformID
                WHERE game_id = @id",
                new { id }
            );
        }

        public async Task<GamePlatforms?> GetByPlatformID(int id)
        {
            return await _db.QuerySingleOrDefaultAsync<GamePlatforms>
            (@" SELECT 
                game_id AS GameID,
                platform_id AS platformID
                WHERE platform_id = @id",
                new { id }
            );
        }


    }
}
