using HackerNews.Api.Models;

namespace HackerNews.Api.Services;

public class HackerNewsService : IHackerNewsService
{
    private readonly ILogger<HackerNewsService> _logger;
    private readonly IHackerNewsApiService _hackerNewsApiService;

    public HackerNewsService(
        ILogger<HackerNewsService> logger,
        IHackerNewsApiService hackerNewsApiService)
    {
        _logger = logger;
        _hackerNewsApiService = hackerNewsApiService;
    }

    public async Task<List<Story>> GetBestStories(int count)
    {
        var ids = await _hackerNewsApiService.GetBestStoriesIds();

        var selectedIds = ids.Take(count);
        var result = new List<Story>(selectedIds.Count());

        // todo parallel it somehow
        foreach (var id in selectedIds)
        {
            var story = await _hackerNewsApiService.GetStoryDetails(id);

            if (story is null)
            {
                _logger.LogWarning($"Skipping story with id='{id}' because it is null.");
                continue;
            }

            result.Add(story);
        }

        return result;
    }
}
