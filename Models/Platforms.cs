namespace Cartridge.Models
{
    public class Platforms
    {
        public int PlatformID { get; set; }
        public string PlatformName {  get; set; }
        public string Slug { get; set; }
        public string Icon { get; set; }

        public string? ShortName { get; set; }
    }
}
