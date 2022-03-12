using speedupApi.Exceptions;
using speedupApi.Models;
using speedupApi.Repositories;
using speedupApi.ViewModels;
using System.Net;

namespace speedupApi.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _repository;
    //private readonly IPriceService _pricesService;
    //private readonly IHttpContextAccessor _httpContextAccessor;
    //private readonly string _apiUrl;
    //private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISelfHttpClient _selfHttpClient;

    public ProductService(IProductRepository repository, ISelfHttpClient selfHttpClient)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      //_pricesService = pricesService ?? throw new ArgumentNullException(nameof(pricesService));
      //_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
      //_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
      //_apiUrl = GetFullyQualifiedApiUrl("/api/prices/prepare/");
      _selfHttpClient = selfHttpClient ?? throw new ArgumentNullException(nameof(selfHttpClient));
    }

    public async Task<ProductViewModel> DeleteProductAsync(int productId)
    {
      Product product = await _repository.DeleteProductAsync(productId);

      if (product == null)
        throw new HttpException(HttpStatusCode.NotFound, "Product not found", $"Product Id: {productId}");

      return new ProductViewModel(product);
    }

    public async Task<IEnumerable<ProductViewModel>> FindProductsAsync(string sku)
    {
      IEnumerable<Product> products = await _repository.FindProductsAsync(sku);
      if (products.Count() == 1)
      {
        //await PreparePricesAsync(products.FirstOrDefault().ProductId);
        ThreadPool.QueueUserWorkItem(delegate
        {
          CallPreparePricesAsync(products.FirstOrDefault().ProductId.ToString());
        });
      }

      return products.Select(p => new ProductViewModel(p));
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
    {
      IEnumerable<Product> products = await _repository.GetAllProductsAsync();
      return products.Select(p => new ProductViewModel(p));
    }

    public async Task<ProductViewModel> GetProductAsync(int productId)
    {
      Product product = await _repository.GetProductAsync(productId);
      if (product == null)
        throw new HttpException(HttpStatusCode.NotFound, "Product not found", $"Product Id: {productId}");
      //await PreparePricesAsync(product.ProductId);
      ThreadPool.QueueUserWorkItem(delegate
      {
        CallPreparePricesAsync(productId.ToString());
      });

      return new ProductViewModel(product);
    }

    private async void CallPreparePricesAsync(string productId)
    {
      await _selfHttpClient.PostIdAsync("products/prepare", productId);
    }
  }
}
