using System;
using System.Collections.Generic;

namespace NAIMS.Models
{
  public class SalesRepDashboardViewModel
  {
    public int OrdersThisMonth { get; set; }
    public decimal PredictedCommission { get; set; }
    public bool HasReachedTarget { get; set; }
    public decimal MonthlyTarget { get; set; }
    public string SalesRepName { get; set; }
    public string EncouragingMessage { get; set; }
    public List<MonthlyOverview> MonthlyOverviews { get; set; }
  }

  public class MonthlyOverview
  {
    public string Month { get; set; }
    public int Orders { get; set; }
    public decimal TotalSales { get; set; }
  }
}
