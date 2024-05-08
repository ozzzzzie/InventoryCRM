using System;
using System.Collections.Generic;

namespace NAIMS.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public long? Barcode { get; set; }

    public string Sku { get; set; } = null!;

    public int? BrandId { get; set; }

    public string Pname { get; set; } = null!;

    public string Size { get; set; } = null!;

    public string Pdescription { get; set; } = null!;

    public double Price { get; set; }

    public double PriceVat { get; set; }

    public int WarehouseQty { get; set; }

    public string WarehouseStatus { get; set; } = null!;

    public int LocalQty { get; set; }

    public string? LocalStatus { get; set; }

    public virtual Brand? Brand { get; set; }
}
