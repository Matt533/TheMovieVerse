namespace MovieVerse.Domain_Layer.Models
{
    public class ActorMovies
    {
        public int ActorId { get; set; }
        public Actor? Actor { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
