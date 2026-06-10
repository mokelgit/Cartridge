using Cartridge.Models;
using Dapper;
using System.Data;

namespace Cartridge.Data
{
    public class CompanyRepository
    {

        private readonly IDbConnection _db;

        public CompanyRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Companies>> GetAllCompanies() 
        {
            return await _db.QueryAsync<Companies>
                (
                @"SELECT 
                id as Id,
                name as Name,
                logo_url as LogoURL,
                founded_date as FoundedDate
                FROM companies
                ORDER BY name ASC

                "
                );
        
        }

        public async Task<GameViewModel?> GetCompanyByName(string name)
        {
            return await _db.QuerySingleOrDefaultAsync<GameViewModel>(
                @"SELECT 
            id as Id,
            name as Name,
            logo_url as LogoURL,
            founded_date as FoundedDate
            FROM companies
            WHERE name = @name",
                new { name }
            );
        }
        public async Task<GameViewModel?> GetCompanyByID(int id)
        {
            return await _db.QuerySingleOrDefaultAsync<GameViewModel>(
                @"SELECT 
            id as Id,
            name as Name,
            logo_url as LogoURL,
            founded_date as FoundedDate
            FROM companies
            WHERE id = @id",
                new { id }
            );
        }

    }
}
