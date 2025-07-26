namespace MovieVerse.Domain_Layer.Models
{
    public class GenreMovies
    {
        public Genre? Genre { get; set; }
        public int GenreId { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
       
    }
}
