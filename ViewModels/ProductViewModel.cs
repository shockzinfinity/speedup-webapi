using speedupApi.Models;

namespace speedupApi.ViewModels
{
  public class ProductViewModel
  {
    public int Id { get; set; }
    public string Sku { get; set; }
    public string Name { get; set; }

    public ProductViewModel(Product product)
    {
      Id = product.ProductId;
      Sku = product.Sku;
      Name = product.Name;
    }
  }
}
