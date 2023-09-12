using HackerNews.Api.Models;

namespace HackerNews.Api.Services;

public class HackerNewsService : IHackerNewsService
{
    // todo: can be moved to appsettings and options
    private const int RequestsBunchSize = 10;

    private readonly IHackerNewsApiService _hackerNewsApiService;

    public HackerNewsService(IHackerNewsApiService hackerNewsApiService)
    {
        _hackerNewsApiService = hackerNewsApiService;
    }

    public async Task<List<Story>> GetBestStories(int count)
    {
        var ids = await _hackerNewsApiService.GetBestStoriesIds();

        var selectedIds = ids.Take(count);
        var result = new List<Story>(selectedIds.Count());

        var tasks = new List<Task<Story>>(10);

        foreach (var id in selectedIds)
        {
            // we are not using await here
            // because we will send a bunch of requests in the ammount of RequestsBunchSize and wait all of them
            var storyTask = _hackerNewsApiService.GetStoryDetails(id);
            tasks.Add(storyTask);

            if (tasks.Count == RequestsBunchSize)
            {
                result.AddRange(await WaitAllAndGetResults(tasks));
            }
        }

        result.AddRange(await WaitAllAndGetResults(tasks));

        return result;
    }

    private async Task<IEnumerable<Story>> WaitAllAndGetResults(List<Task<Story>> tasks)
    {
        await Task.WhenAll(tasks);
        var stories = tasks.Select(x => x.Result).ToList();
        tasks.Clear();

        return stories;
    }
}
