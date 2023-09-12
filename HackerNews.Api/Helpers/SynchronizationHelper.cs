using System.Collections.Concurrent;

namespace HackerNews.Api.Helpers;

public static class SynchronizationHelper
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _syncCollection = new();

    public static Task Lock(
        int key,
        Func<Task> asyncAction)
    {
        return Lock(key.ToString(), asyncAction);
    }

    public static async Task Lock(
        string key,
        Func<Task> asyncAction)
    {
        var semaphorSlim = _syncCollection.GetOrAdd(key, id => new SemaphoreSlim(1, 1));

        await semaphorSlim.WaitAsync();
        try
        {
            await asyncAction();
        }
        finally
        {
            semaphorSlim.Release();
        }
    }
}
