using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using speedupApi.Data;
using speedupApi.Models;
using System.Globalization;
using System.Text.Json;

namespace speedupApi.Repositories
{
  public class PriceRepository : IPriceRepository
  {
    private readonly DefaultContext _context;
    private readonly Settings _settings;
    private readonly IDistributedCache _distributedCache;

    public PriceRepository(DefaultContext context, IConfiguration configuration, IDistributedCache distributedCache)
    {
      _settings = new Settings(configuration);
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
    }

    public async Task<IEnumerable<Price>> GetPricesAsync(int prodcutId)
    {
      //return await _context.Prices.Where(p => p.ProductId == prodcutId).ToListAsync();

      IEnumerable<Price> prices = null;
      string cacheKey = "Prices: " + prodcutId;
      var pricesTemp = await _distributedCache.GetStringAsync(cacheKey);
      if (pricesTemp != null)
      {
        prices = JsonSerializer.Deserialize<IEnumerable<Price>>(pricesTemp);
      }
      else
      {
        prices = await _context.Prices.FromSqlRaw("[dbo].[GetPricesByProductId] @productId = {0}", prodcutId).ToListAsync();

        DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.PricesExpirationPeriod));
        await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(prices), cacheOptions);
      }
      return prices;
    }

    public async Task PreparePricesAsync(int productId)
    {
      IEnumerable<Price> prices = null;
      string cacheKey = "Prices: " + productId;

      var pricesTemp = await _distributedCache.GetStringAsync(cacheKey);
      if (pricesTemp != null)
      {
        return; // already cached
      }
      else
      {
        prices = await _context.Prices.FromSqlRaw("[dbo].[GetPricesByProductId] @productId = {0}", productId).ToListAsync();
        DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.PricesExpirationPeriod));
        await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(prices), cacheOptions);
      }
      return;
    }

    private class Settings
    {
      public int PricesExpirationPeriod = 15;
      public Settings(IConfiguration configuration)
      {
        int pricesExpirationPeriod;
        if (int.TryParse(configuration["Caching:PricesExpirationPeriod"], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out pricesExpirationPeriod))
        {
          PricesExpirationPeriod = pricesExpirationPeriod;
        }
      }
    }
  }
}
