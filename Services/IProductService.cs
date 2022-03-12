using Microsoft.AspNetCore.Mvc;
using speedupApi.ViewModels;

namespace speedupApi.Services
{
  public interface IProductService
  {
    //Task<IActionResult> GetAllProductsAsync();
    //Task<IActionResult> GetProductAsync(int productId);
    //Task<IActionResult> FindProductsAsync(string sku);
    //Task<IActionResult> DeleteProductAsync(int productId);
    Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
    Task<ProductViewModel> GetProductAsync(int productId);
    Task<IEnumerable<ProductViewModel>> FindProductsAsync(string sku);
    Task<ProductViewModel> DeleteProductAsync(int productId);
  }
}
