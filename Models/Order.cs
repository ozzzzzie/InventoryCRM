using System;
using System.Collections.Generic;

namespace NAIMS.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? ContactId { get; set; }

    public DateOnly OrderDate { get; set; }

    public string? SalesRep { get; set; }

    public virtual Contact? Contact { get; set; }
}
