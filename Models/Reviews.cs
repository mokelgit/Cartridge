namespace Cartridge.Models
{
    public class Reviews
    {

        public int GameID { get; set; }
        public string UserID { get; set; }
        public double Rating { get; set; }
        public string ReviewBody { get; set; }
        public DateTime ReviewDate { get; set; }

        public string Username { get; set; }
    }
}
