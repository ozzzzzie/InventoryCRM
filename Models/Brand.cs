using System;
using System.Collections.Generic;

namespace NAIMS.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string Bname { get; set; } = null!;

    public string Bdescription { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
