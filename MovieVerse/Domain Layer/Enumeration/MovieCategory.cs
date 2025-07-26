using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MovieVerse.Domain_Layer.Enumeration
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MovieCategory
    {
        [EnumMember(Value = "NowPlaying")]
        NowPlaying = 1,
        [EnumMember(Value = "Popular")]
        Popular = 2,
        [EnumMember(Value = "TopRated")]
        TopRated = 3,
        [EnumMember(Value = "Upcoming")]
        Upcoming = 4
    }
}
