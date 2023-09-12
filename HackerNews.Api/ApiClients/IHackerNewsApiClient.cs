using HackerNews.Api.Models;
using Refit;

namespace HackerNews.Api.ApiClients;

public interface IHackerNewsApiClient
{
    [Get("/v0/beststories.json")]
    Task<List<int>> GetBestStoriesIds();

    [Get("/v0/item/{id}.json")]
    Task<Story> GetStoryDetails(int id);
}
