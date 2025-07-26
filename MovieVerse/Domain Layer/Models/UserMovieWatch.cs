namespace MovieVerse.Domain_Layer.Models
{
    public class UserMovieWatch
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public DateTime WatchedOn { get; set; }
    }
}
