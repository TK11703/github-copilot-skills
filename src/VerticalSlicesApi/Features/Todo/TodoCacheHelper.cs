using Microsoft.Extensions.Caching.Memory;

namespace VerticalSlicesApi.Features.Todo;

internal static class TodoCacheHelper
{
    internal const string CacheKey = "todo_items";

    internal static List<TodoItem> GetItems(IMemoryCache cache)
    {
        return cache.GetOrCreate(CacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromHours(1);
            return new List<TodoItem>();
        }) ?? [];
    }

    internal static void SetItems(IMemoryCache cache, List<TodoItem> items)
    {
        cache.Set(CacheKey, items, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(1)
        });
    }
}
