namespace MovieVerse.Domain_Layer.DTOs
{
    public record MovieDto
    {
        public int Id { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty; 
        public List<string> Genres { get; set; } = new List<string>();
    }
}
