using System;
using System.Collections.Generic;

namespace NAIMS.Models
{
  public partial class Order
  {
    public int OrderId { get; set; }
    public int? ContactId { get; set; }
    public DateOnly OrderDate { get; set; }
    public int? EmployeeId { get; set; }

    public virtual Contact? Contact { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<ProductsOrder> ProductsOrders { get; set; } = new List<ProductsOrder>();
  }
}
