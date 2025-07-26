using System.Text.Json.Serialization;
using MovieVerse.Application_Layer.DTOs.ActorDto;

namespace MovieVerse.Domain_Layer.Helper
{
    public class TmdbActorOverviewResponse
    {
        [JsonPropertyName("results")]
        public List<ActorOverviewDto> Results { get; set; } = new List<ActorOverviewDto>();
    }
}
