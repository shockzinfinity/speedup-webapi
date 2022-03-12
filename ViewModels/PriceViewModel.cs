using speedupApi.Models;

namespace speedupApi.ViewModels
{
  public class PriceViewModel
  {
    public decimal Price { get; set; }
    public string Supplier { get; set; }

    public PriceViewModel(Price price)
    {
      Price = price.Value;
      Supplier = price.Supplier;
    }
  }
}
