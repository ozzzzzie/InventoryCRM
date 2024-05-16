using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NAIMS.Models
{
  public class AddProductsViewModel
  {
    public Order Order { get; set; }
    public List<ProductsOrder> ProductsOrders { get; set; } = new List<ProductsOrder>();

    [BindNever]
    public List<SelectListItem> Products { get; set; }

    [BindNever]
    public Dictionary<int, double> ProductPrices { get; set; }

    public List<int> ProductsOrdersToRemove { get; set; } = new List<int>();
  }
}
