using Microsoft.EntityFrameworkCore;
using speedupApi.Data;
using speedupApi.Models;

namespace speedupApi.Repositories
{
  public class PriceRepository : IPriceRepository
  {
    private readonly DefaultContext _context;

    public PriceRepository(DefaultContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Price>> GetPricesAsync(int prodcutId)
    {
      return await _context.Prices.FromSqlRaw("[dbo].[GetPricesByProductId] @productId = {0}", prodcutId).ToListAsync();

    }
  }
}
