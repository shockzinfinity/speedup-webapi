using Microsoft.AspNetCore.Mvc;
using speedupApi.Models;
using speedupApi.Repositories;
using speedupApi.ViewModels;

namespace speedupApi.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IActionResult> DeleteProductAsync(int productId)
    {
      try
      {
        Product product = await _repository.DeleteProductAsync(productId);

        if (product != null)
        {
          return new OkObjectResult(new ProductViewModel()
          {
            Id = product.ProductId,
            Sku = product.Sku.Trim(),
            Name = product.Name.Trim()
          });
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

    public async Task<IActionResult> FindProductsAsync(string sku)
    {
      try
      {
        IEnumerable<Product> products = await _repository.FindProductsAsync(sku);
        if (products != null)
        {
          return new OkObjectResult(products.Select(p => new ProductViewModel()
          {
            Id = p.ProductId,
            Sku = p.Sku.Trim(),
            Name = p.Name.Trim()
          }));
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

    public async Task<IActionResult> GetAllProductsAsync()
    {
      try
      {
        IEnumerable<Product> products = await _repository.GetAllProductsAsync();
        if (products != null)
        {
          return new OkObjectResult(products.Select(p => new ProductViewModel()
          {
            Id = p.ProductId,
            Sku = p.Sku.Trim(),
            Name = p.Name.Trim()
          }));
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

    public async Task<IActionResult> GetProductAsync(int productId)
    {
      try
      {
        Product product = await _repository.GetProductAsync(productId);
        if (product != null)
        {
          return new OkObjectResult(new ProductViewModel()
          {
            Id = product.ProductId,
            Sku = product.Sku.Trim(),
            Name = product.Name.Trim()
          });
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
  }
}
