using Microsoft.AspNetCore.Mvc;
using speedupApi.Models;
using speedupApi.Repositories;
using speedupApi.ViewModels;

namespace speedupApi.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _repository;
    //private readonly IPriceService _pricesService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiUrl;

    public ProductService(IProductRepository repository, /*IPriceService pricesService*/ IHttpContextAccessor httpContextAccessor)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      //_pricesService = pricesService ?? throw new ArgumentNullException(nameof(pricesService));
      _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

      _apiUrl = GetFullyQualifiedApiUrl("/api/prices/prepare/");
    }

    private string GetFullyQualifiedApiUrl(string apiRout)
    {
      string apiUrl = string.Format("{0}://{1}{2}", _httpContextAccessor.HttpContext.Request.Scheme, _httpContextAccessor.HttpContext.Request.Host, apiRout);

      return apiUrl;
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
          if (products.Count() == 1)
          {
            //await PreparePricesAsync(products.FirstOrDefault().ProductId);
            ThreadPool.QueueUserWorkItem(delegate
            {
              PreparePricesAsync(products.FirstOrDefault().ProductId);
            });
          }

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
          //await PreparePricesAsync(product.ProductId);
          ThreadPool.QueueUserWorkItem(delegate
          {
            PreparePricesAsync(productId);
          });

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

    private async void PreparePricesAsync(int productId)
    {
      //await _pricesService.PreparePricesAsync(productId);
      using (HttpClient client = new HttpClient())
      {
        var parameters = new Dictionary<string, string>();
        var encodedContent = new FormUrlEncodedContent(parameters);

        try
        {
          var result = await client.PostAsync(_apiUrl + productId, encodedContent).ConfigureAwait(false);
        }
        catch
        {
        }
      }
    }
  }
}
