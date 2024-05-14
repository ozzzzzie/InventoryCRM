using System;
using System.Collections.Generic;

namespace NAIMS.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EFirstname { get; set; } = null!;

    public string ELastname { get; set; } = null!;

    public string ERole { get; set; } = null!;

    public string EEmail { get; set; } = null!;

    public string EPhonenumber { get; set; } = null!;

    public int ETarget { get; set; }

    public decimal EComissionPerc { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
