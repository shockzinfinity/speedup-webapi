using Microsoft.AspNetCore.Mvc;

namespace speedupApi.Services
{
  public interface IPriceService
  {
    Task<IActionResult> GetPricesAsync(int productId);
  }
}
