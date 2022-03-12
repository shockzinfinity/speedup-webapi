using Microsoft.AspNetCore.Mvc;

namespace speedupApi.Services
{
  public interface IProductService
  {
    Task<IActionResult> GetAllProductsAsync();
    Task<IActionResult> GetProductAsync(int productId);
    Task<IActionResult> FindProductsAsync(string sku);
    Task<IActionResult> DeleteProductAsync(int productId);
  }
}
