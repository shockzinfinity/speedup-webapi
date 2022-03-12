using speedupApi.ViewModels;

namespace speedupApi.Services
{
  public interface IPriceService
  {
    Task<IEnumerable<PriceViewModel>> GetPricesAsync(int productId);
    Task<bool> IsPriceCachedAsync(int productId);
    Task RemovePriceAsync(int productId);
    Task PreparePricesAsync(int productId);
  }
}
