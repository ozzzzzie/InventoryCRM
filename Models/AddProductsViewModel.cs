namespace NAIMS.Models
{
  public class AddProductsViewModel
  {
    public Order Order { get; set; }
    public ProductsOrder ProductsOrder { get; set; }
    public IEnumerable<Product>? Products { get; set; }


    //public IEnumerable<Order> Orders { get; set; }
  }
}
