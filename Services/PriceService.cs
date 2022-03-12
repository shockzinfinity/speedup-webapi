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

    public async Task<IEnumerable<PriceViewModel>> GetPricesAsync(int productId)
    {
      IEnumerable<Price> prices = await _repository.GetPricesAsync(productId);
      return prices.Select(p => new PriceViewModel(p)).OrderBy(p => p.Price).ThenBy(p => p.Supplier);
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
