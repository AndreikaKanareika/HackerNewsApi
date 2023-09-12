using HackerNews.Api.ApiClients;
using HackerNews.Api.Helpers;
using HackerNews.Api.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HackerNews.Api.Services;

public class CachedHackerNewsApiService : IHackerNewsApiService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHackerNewsApiClient _hackerNewsApiClient;
    private readonly CacheOptions _cacheOptions;

    public CachedHackerNewsApiService(
        IMemoryCache memoryCache,
        IHackerNewsApiClient hackerNewsApiClient,
        IOptions<CacheOptions> cacheOptions)
    {
        _memoryCache = memoryCache;
        _hackerNewsApiClient = hackerNewsApiClient;
        _cacheOptions = cacheOptions.Value;
    }

    public async Task<List<int>> GetBestStoriesIds()
    {
        // if data is cached just return it
        if (_memoryCache.TryGetValue<List<int>>(CacheKeys.BestStoriesIds, out var ids))
        {
            return ids;
        }

        // as we dont want to overload HackerNews API
        // we need to send request only once, even if we have hundrends request on our side
        // in this case we lock by key CacheKeys.BestStoriesIds
        // and inside of lock checking the cache again (see Double-checked locking pattern)
        // if data is not present in cache then send request to HackerNews API
        await SynchronizationHelper.Lock(
            key: CacheKeys.BestStoriesIds,
            asyncAction: async () =>
            {
                if (!_memoryCache.TryGetValue(CacheKeys.BestStoriesIds, out ids))
                {
                    ids = await _hackerNewsApiClient.GetBestStoriesIds();
                    _memoryCache.Set(CacheKeys.BestStoriesIds, ids, TimeSpan.FromSeconds(_cacheOptions.BestStoriesIdsTtlInSeconds));
                }
            });

        return ids;
    }

    public async Task<Story> GetStoryDetails(int id)
    {
        if (_memoryCache.TryGetValue<Story>(id, out var story))
        {
            return story;
        }

        await SynchronizationHelper.Lock(
            key: id,
            asyncAction: async () =>
            {
                if (!_memoryCache.TryGetValue(id, out story))
                {
                    story = await _hackerNewsApiClient.GetStoryDetails(id);
                    _memoryCache.Set(id, story, TimeSpan.FromSeconds(_cacheOptions.StoryDetailsTtlInSeconds));
                }
            });

        return story;
    }
}
