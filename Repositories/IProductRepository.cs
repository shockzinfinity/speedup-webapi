using speedupApi.Models;

namespace speedupApi.Repositories
{
  public interface IProductRepository
  {
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> GetProductAsync(int productId);
    Task<IEnumerable<Product>> FindProductsAsync(string sku);
    Task<Product> DeleteProductAsync(int productId);
  }
}
