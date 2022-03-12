using speedupApi.Models;

namespace speedupApi.Repositories
{
  public interface IPriceRepository
  {
    Task<IEnumerable<Price>> GetPricesAsync(int prodcutId);
    Task PreparePricesAsync(int productId);
  }
}
