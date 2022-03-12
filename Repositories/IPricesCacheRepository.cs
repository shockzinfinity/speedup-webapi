using Microsoft.Extensions.Caching.Distributed;
using speedupApi.Models;

namespace speedupApi.Repositories
{
  public interface IPricesCacheRepository
  {
    Task<IEnumerable<Price>> GetOrSetValueAsync(string key, Func<Task<IEnumerable<Price>>> valueDelegate, DistributedCacheEntryOptions options = null);
    Task<bool> IsValueCachedAsync(string key);
    Task RemoveValueAsync(string key);
  }
}
