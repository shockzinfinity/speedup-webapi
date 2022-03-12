using Microsoft.AspNetCore.Mvc;
using speedupApi.Services;

namespace speedupApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PricesController : ControllerBase
  {
    private readonly IPriceService _service;

    public PricesController(IPriceService service)
    {
      _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetpricesAsync(int id)
    {
      return new OkObjectResult(await _service.GetPricesAsync(id));
    }

    // POST api/prices/prepare/5
    [HttpPost("prepare/{id}")]
    public async Task<IActionResult> PreparePricessAsync(int id)
    {
      await _service.PreparePricesAsync(id);

      return Ok();
    }
  }
}
