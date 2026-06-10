using Cartridge.Models;
using Dapper;
using System.Data;

namespace Cartridge.Data
{
    public class PlatformRepository
    {

        private readonly IDbConnection _db;

        public PlatformRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Platforms>> GetAllPlatforms()
        {
            return await _db.QueryAsync<Platforms>
                (@"SELECT
                platform_id AS PlatformID,
                platform AS Platform,
                slug AS Slug,
                icon AS Icon
                FROM platforms
                ORDER BY platform ASC
                ");
        }

        public async Task<Platforms?> GetPlatformByID(int id) 
        {
            return await _db.QuerySingleOrDefaultAsync<Platforms>
            (@"SELECT
                platform_id AS PlatformID,
                platform AS Platform,
                slug AS Slug,
                icon AS Icon
                FROM platforms
                WHERE id = @id",
                new { id }
            );
           
        }

        public async Task<Platforms?> GetPlatformByName(string platform)
        {
            return await _db.QuerySingleOrDefaultAsync<Platforms>
            (@"SELECT
                platform_id AS PlatformID,
                platform AS Platform,
                slug AS Slug,
                icon AS Icon
                FROM platforms
                WHERE platform = @platform",
                new { platform }
            );

        }
    }
}
