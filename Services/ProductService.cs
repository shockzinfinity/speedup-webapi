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
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IProductRepository repository, /*IPriceService pricesService*/ IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      //_pricesService = pricesService ?? throw new ArgumentNullException(nameof(pricesService));
      _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
      _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
      _apiUrl = GetFullyQualifiedApiUrl("/api/prices/prepare/");
    }

    private string GetFullyQualifiedApiUrl(string apiRout)
    {
      string apiUrl = string.Format("{0}://{1}{2}", _httpContextAccessor.HttpContext.Request.Scheme, _httpContextAccessor.HttpContext.Request.Host, apiRout);

      return apiUrl;
    }

    public async Task<IActionResult> DeleteProductAsync(int productId)
    {
      Product product = await _repository.DeleteProductAsync(productId);

      if (product != null)
      {
        return new OkObjectResult(new ProductViewModel(product));
      }
      else
      {
        return new NotFoundResult();
      }
    }

    public async Task<IActionResult> FindProductsAsync(string sku)
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

        return new OkObjectResult(products.Select(p => new ProductViewModel(p)));
      }
      else
      {
        return new NotFoundResult();
      }
    }

    public async Task<IActionResult> GetAllProductsAsync()
    {
      IEnumerable<Product> products = await _repository.GetAllProductsAsync();
      if (products != null)
      {
        return new OkObjectResult(products.Select(p => new ProductViewModel(p)));
      }
      else
      {
        return new NotFoundResult();
      }
    }

    public async Task<IActionResult> GetProductAsync(int productId)
    {
      Product product = await _repository.GetProductAsync(productId);
      if (product != null)
      {
        //await PreparePricesAsync(product.ProductId);
        ThreadPool.QueueUserWorkItem(delegate
        {
          PreparePricesAsync(productId);
        });

        return new OkObjectResult(new ProductViewModel(product));
      }
      else
      {
        return new NotFoundResult();
      }
    }

    private async void PreparePricesAsync(int productId)
    {
      //await _pricesService.PreparePricesAsync(productId);

      var parameters = new Dictionary<string, string>();
      var encodedContent = new FormUrlEncodedContent(parameters);

      try
      {
        HttpClient client = _httpClientFactory.CreateClient();
        var result = await client.PostAsync(_apiUrl + productId, encodedContent).ConfigureAwait(false);
      }
      catch
      {
      }
    }
  }
}
