using Microsoft.EntityFrameworkCore;

namespace Cartridge.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
    }
}
