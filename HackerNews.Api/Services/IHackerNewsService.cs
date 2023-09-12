using HackerNews.Api.Models;

namespace HackerNews.Api.Services;

public interface IHackerNewsService
{
    Task<List<Story>> GetBestStories(int count);
}
