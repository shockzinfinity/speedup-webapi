using Microsoft.AspNetCore.Mvc;
using speedupApi.Models;
using speedupApi.Repositories;
using speedupApi.ViewModels;

namespace speedupApi.Services
{
  public class PriceService : IPriceService
  {
    private readonly IPriceRepository _repository;

    public PriceService(IPriceRepository repository)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IActionResult> GetPricesAsync(int productId)
    {
      try
      {
        IEnumerable<Price> prices = await _repository.GetPricesAsync(productId);
        if (prices != null)
        {
          return new OkObjectResult(prices.Select(p => new PriceViewModel
          {
            Price = p.Value,
            Supplier = p.Supplier.Trim()
          }).OrderBy(p => p.Price).ThenBy(p => p.Supplier));
        }
        else
        {
          return new NotFoundResult();
        }
      }
      catch
      {
        return new ConflictResult();
      }
    }

    public async Task PreparePricesAsync(int productId)
    {
      try
      {
        await _repository.PreparePricesAsync(productId);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
