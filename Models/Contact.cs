using System;
using System.Collections.Generic;

namespace NAIMS.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string Cname { get; set; } = null!;

    public string Ctype { get; set; } = null!;

    public string Caddress { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
