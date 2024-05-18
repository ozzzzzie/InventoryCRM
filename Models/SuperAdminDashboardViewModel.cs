using System.Collections.Generic;

namespace NAIMS.Models
{
  public class SuperAdminDashboardViewModel
  {
    public int TotalOrders { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalTax { get; set; }
    public List<AMonthlyOverview> MonthlyOverviews { get; set; }
    public List<ASalesForecast> ASalesForecasts { get; set; }
    public List<AEmployeePerformance> EmployeePerformances { get; set; }
  }

  public class AMonthlyOverview
  {
    public string Month { get; set; }
    public int Orders { get; set; }
    public decimal TotalSales { get; set; }
  }

  public class ASalesForecast
  {
    public string Month { get; set; }
    public decimal PredictedSales { get; set; }
  }

  public class AEmployeePerformance
  {
    public string EmployeeName { get; set; }
    public decimal Sales { get; set; }
    public decimal Target { get; set; }
    public bool HasReachedTarget { get; set; }
  }
}
