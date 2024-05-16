using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NAIMS.Models
{
  public partial class ProductsOrder
  {
    public int ProductorderId { get; set; }
    public int? OrderId { get; set; }
    public int ProductId { get; set; }
    public int Qty { get; set; }

    [BindNever]
    public virtual Order Order { get; set; }

    [BindNever]
    public virtual Product Product { get; set; }
  }
}
