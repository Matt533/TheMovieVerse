using System.Text.Json.Serialization;

namespace MovieVerse.Application_Layer.DTOs.ActorDto
{
    public class ActorOverviewDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
  
        [JsonPropertyName("profile_path")]
        public string Avatar { get; set; } = string.Empty;
    }
}
