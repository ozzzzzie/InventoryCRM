﻿using System;
using System.Collections.Generic;

namespace NAIMS.Models;

public partial class ProductsOrder
{
    public int ProductorderId { get; set; }

    public int? OrderId { get; set; }

    public int ProductId { get; set; }

    public int Qty { get; set; }
}
