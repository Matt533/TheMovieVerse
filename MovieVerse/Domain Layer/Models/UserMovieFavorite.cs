namespace MovieVerse.Domain_Layer.Models
{
    public class UserMovieFavorite
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie Movie  { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
