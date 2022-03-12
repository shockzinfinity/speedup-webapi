using speedupApi.ViewModels;

namespace speedupApi.Services
{
  public interface IPriceService
  {
    //Task<IActionResult> GetPricesAsync(int productId);
    Task<IEnumerable<PriceViewModel>> GetPricesAsync(int productId);
    Task PreparePricesAsync(int productId);
  }
}
