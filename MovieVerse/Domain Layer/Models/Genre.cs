using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieVerse.Domain_Layer.Models
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<GenreMovies> GenreMovies { get; set; } = new List<GenreMovies>();
    }
}
