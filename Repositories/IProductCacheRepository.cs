using Microsoft.Extensions.Caching.Distributed;
using speedupApi.Models;

namespace speedupApi.Repositories
{
  public interface IProductCacheRepository
  {
    Task<Product> GetOrSetValueAsync(string key, Func<Task<Product>> valueDelegate, DistributedCacheEntryOptions options = null);
    Task<bool> IsValueCachedAsync(string key);
    Task RemoveValueAsync(string key);
    Task SetValueAsync(string key, Product value, DistributedCacheEntryOptions options = null);
  }
}
