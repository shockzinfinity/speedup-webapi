using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using speedupApi.Models;
using speedupApi.Settings;

namespace speedupApi.Repositories
{
  public class PriceCacheRepository : DistributedCacheRepository<IEnumerable<Price>>, IPricesCacheRepository
  {
    private const string KeyPrefix = "Prices: ";
    private readonly PricesSettings _settings;

    public PriceCacheRepository(IDistributedCache distributedCache, IOptions<PricesSettings> settings) : base(distributedCache, KeyPrefix)
    {
      _settings = settings.Value;
    }

    public override Task<IEnumerable<Price>> GetOrSetValueAsync(string key, Func<Task<IEnumerable<Price>>> valueDelegate, DistributedCacheEntryOptions options = null)
    {
      return base.GetOrSetValueAsync(key, valueDelegate, options);
    }

    protected override DistributedCacheEntryOptions GetDefaultOptions()
    {
      return new DistributedCacheEntryOptions()
      {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_settings.CachingExpirationPeriod)
      };
    }
  }
}
