using Cartridge.Models;
using Dapper;
using System.Data;

namespace Cartridge.Data
{
    public class GameCompRepo
    {

        private readonly IDbConnection _db;

        public GameCompRepo(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GameCompanies>> GetAllGameCompanies()
        {
            return await _db.QueryAsync<GameCompanies>
                (@"SELECT
                game_id AS GameID,
                company_id AS CompanyID,
                is_publisher as IsPublisher,
                is_developer as IsDeveloper
                FROM game_companies
                ");
        }

        public async Task<IEnumerable<GameCompanies?>> GetByGameID(int id)
        {
            return await _db.QueryAsync<GameCompanies>
            (@" SELECT 
                game_id AS GameID,
                company_id AS CompanyID,
                is_publisher as IsPublisher,
                is_developer as IsDeveloper
                FROM game_companies
                WHERE game_id = @id",
                new { id }
            );
        }

        public async Task<IEnumerable<GameCompanies?>> GetByCompanyID(int id)
        {
            return await _db.QueryAsync<GameCompanies>
            (@" SELECT 
                game_id AS GameID,
                company_id AS CompanyID,
                is_publisher as IsPublisher,
                is_developer as IsDeveloper
                FROM game_companies
                WHERE company_id = @id",
                new { id }
            );
        }
    }
}
