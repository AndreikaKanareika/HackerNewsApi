namespace HackerNews.Api.Models;

public class StoryResponseDto
{
    public string? Title { get; set; }
    public Uri? Uri { get; set; }
    public string? PostedBy { get; set; }
    public DateTime Time { get; set; }
    public int Score { get; set; }
    public int CommentsCount { get; set; }
}
