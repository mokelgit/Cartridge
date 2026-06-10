using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Cartridge.Models;

namespace Cartridge.Models
{
    public class GameViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }      // nullable since it can be NULL
        public string? BackgroundImageURL { get; set; }  // nullable since not all games have images
        public string? CoverImageURL { get; set; }
        public string? Slug { get; set; }

        public List<Companies> Publisher { get; set; } = new();

        public List<Companies> Developer { get; set; } = new();
    }
}
