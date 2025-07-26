using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieVerse.Domain_Layer.Models
{
    public class Actor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string Birthday { get; set; } = string.Empty;
        public string ProfilePath {  get; set; } = string.Empty;
        public List<ActorMovies> PlayedInMovies { get; set; } = new List<ActorMovies>();
    }
}
