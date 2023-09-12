using Newtonsoft.Json;

namespace HackerNews.Api.Models;

public class Story
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("url")]
    public Uri? Uri { get; set; }

    [JsonProperty("by")]
    public string? PostedBy { get; set; }

    [JsonProperty("time")]
    public int Time { get; set; }

    [JsonProperty("score")]
    public int Score { get; set; }

    [JsonProperty("descendants")]
    public int CommentsCount { get; set; }
}
