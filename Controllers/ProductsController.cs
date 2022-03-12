using Microsoft.AspNetCore.Mvc;
using speedupApi.Services;

namespace speedupApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
      _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProductsAsync()
    {
      return new OkObjectResult(await _service.GetAllProductsAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductAsync(int id)
    {
      return new OkObjectResult(await _service.GetProductAsync(id));
    }

    [HttpGet("find/{sku}")]
    public async Task<IActionResult> FindProductsAsync(string sku)
    {
      return new OkObjectResult(await _service.FindProductsAsync(sku));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
      return new OkObjectResult(await _service.DeleteProductAsync(id));
    }
  }
}
