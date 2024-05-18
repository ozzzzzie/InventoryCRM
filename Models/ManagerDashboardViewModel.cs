using System.Collections.Generic;

namespace NAIMS.Models
{
  public class ManagerDashboardViewModel
  {
    public int TotalOrdersThisMonth { get; set; }
    public decimal TotalSalesThisMonth { get; set; }
    public decimal TaxCollectedThisYear { get; set; }
    public List<SalesForecast> SalesForecast { get; set; }
    public List<ProductSalesTrend> ProductSalesTrends { get; set; }
    public List<EmployeeTargetStatus> EmployeeTargetStatuses { get; set; }
    public List<Product> OutOfStockItems { get; set; }

    public decimal TotalSalesThisYear { get; set; }
    public decimal Target { get; set; }
    public bool HasReachedTarget { get; set; }
  }

  public class SalesForecast
  {
    public string Month { get; set; }
    public decimal PredictedSales { get; set; }
  }

  public class ProductSalesTrend
  {
    public string ProductName { get; set; }
    public List<MonthlySales> MonthlySales { get; set; }
  }

  public class MonthlySales
  {
    public string Month { get; set; }
    public int Quantity { get; set; }
    public decimal TotalSales { get; set; }
  }

  public class EmployeeTargetStatus
  {
    public string EmployeeName { get; set; }
    public bool HasReachedTarget { get; set; }
    public decimal Sales { get; set; }
    public decimal Target { get; set; }
  }
}
