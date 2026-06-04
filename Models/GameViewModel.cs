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
        public string? Publisher { get; set; }
        public string? Developer { get; set; }
    }
}
