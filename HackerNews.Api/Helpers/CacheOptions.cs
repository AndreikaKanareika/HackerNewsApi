namespace HackerNews.Api.Helpers;

public class CacheOptions
{
    public int QueryResultTtlInSeconds { get; set; }
    public int BestStoriesIdsTtlInSeconds { get; set; }
    public int StoryDetailsTtlInSeconds { get; set; }
}
