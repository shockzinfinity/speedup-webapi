using speedupApi.Models;
using speedupApi.Repositories;
using speedupApi.ViewModels;

namespace speedupApi.Services
{
  public class PriceService : IPriceService
  {
    private readonly IPriceRepository _repository;
    private readonly IPricesCacheRepository _pricesCacheRepository;

    public PriceService(IPriceRepository repository, IPricesCacheRepository pricesCacheRepository)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _pricesCacheRepository = pricesCacheRepository ?? throw new ArgumentNullException(nameof(pricesCacheRepository));
    }

    public async Task<IEnumerable<PriceViewModel>> GetPricesAsync(int productId)
    {
      IEnumerable<Price> prices = await _pricesCacheRepository.GetOrSetValueAsync(productId.ToString(), async () => await _repository.GetPricesAsync(productId));

      return prices.Select(p => new PriceViewModel(p)).OrderBy(p => p.Price).ThenBy(p => p.Supplier);
    }

    public async Task<bool> IsPriceCachedAsync(int productId)
    {
      return await _pricesCacheRepository.IsValueCachedAsync(productId.ToString());
    }

    public async Task PreparePricesAsync(int productId)
    {
      try
      {
        await _pricesCacheRepository.GetOrSetValueAsync(productId.ToString(), async () => await _repository.GetPricesAsync(productId));
      }
      catch (Exception ex)
      {
      }
    }

    public async Task RemovePriceAsync(int productId)
    {
      await _pricesCacheRepository.RemoveValueAsync(productId.ToString());
    }
  }
}
