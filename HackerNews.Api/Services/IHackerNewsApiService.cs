using HackerNews.Api.Models;

namespace HackerNews.Api.Services;

public interface IHackerNewsApiService
{
    Task<List<int>> GetBestStoriesIds();
    Task<Story> GetStoryDetails(int id);
}
