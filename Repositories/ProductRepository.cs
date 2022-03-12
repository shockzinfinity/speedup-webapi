using Microsoft.EntityFrameworkCore;
using speedupApi.Data;
using speedupApi.Models;

namespace speedupApi.Repositories
{
  public class ProductRepository : IProductRepository
  {
    private readonly DefaultContext _context;

    public ProductRepository(DefaultContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Product> DeleteProductAsync(int productId)
    {
      Product product = await GetProductAsync(productId);
      if (product != null)
      {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
      }

      return product;
    }

    public async Task<IEnumerable<Product>> FindProductsAsync(string sku)
    {
      //return await _context.Products.Where(p => p.Sku == sku).ToListAsync();
      //return await _context.Products.FromSqlRaw("[dbo].[GetProductBySKUError] @sku = {0}", sku).ToListAsync();
      return await _context.Products.FromSqlRaw("[dbo].[GetProductBySKU] @sku = {0}", sku).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
      return await _context.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product> GetProductAsync(int productId)
    {
      return await _context.Products.AsNoTracking().Where(p => p.ProductId == productId).FirstOrDefaultAsync();
    }
  }
}
