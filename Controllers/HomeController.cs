using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcStarter.Models;
using Microsoft.AspNetCore.Authorization;
using NAIMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace AspnetCoreMvcStarter.Controllers;

public class HomeController : Controller
{
  private readonly NaimsdbContext _context;

  public HomeController(NaimsdbContext context)
  {
    _context = context;
  }

  [Authorize]
  public IActionResult Index()
  {
    // check the user's role and redirect them to the appropriate dashboard
    if (User.IsInRole("SuperAdmin"))
    {
      return RedirectToAction("SuperAdminDashboard");
    }
    else if (User.IsInRole("Manager"))
    {
      return RedirectToAction("ManagerDashboard");
    }
    else if (User.IsInRole("Sales"))
    {
      return RedirectToAction("SalesDashboard");
    }
    else
    {
      // if the user's role undefined, redirect them to main page
      return View();
    }
  }

  public IActionResult SuperAdminDashboard()
  {
    var employees = _context.Employees.ToList();

    return View(employees);
  }

  public async Task<IActionResult> ManagerDashboard()
  {
    var currentMonth = DateTime.Now.Month;
    var currentYear = DateTime.Now.Year;

    var ordersThisMonth = await _context.Orders
        .Include(o => o.ProductsOrders)
        .ThenInclude(po => po.Product)
        .Where(o => o.OrderDate.Year == currentYear && o.OrderDate.Month == currentMonth)
        .ToListAsync();

    var totalOrdersThisMonth = ordersThisMonth.Count;
    var totalSalesThisMonth = ordersThisMonth.Sum(o => o.ProductsOrders.Sum(po => po.Qty * po.Product.Price));

    var allOrdersThisYear = await _context.Orders
        .Include(o => o.ProductsOrders)
        .ThenInclude(po => po.Product)
        .Where(o => o.OrderDate.Year == currentYear)
        .ToListAsync();

    var taxCollectedThisYear = allOrdersThisYear.Sum(o => o.ProductsOrders.Sum(po => po.Qty * (int)po.Product.Price * 0.1m));

    var salesForecast = await GenerateSalesForecast();
    var productSalesTrends = await GetProductSalesTrends();
    var employeeTargetStatuses = await GetEmployeeTargetStatuses();
    var outOfStockItems = await _context.Products.Where(p => p.WarehouseQty == 0).ToListAsync();

    var viewModel = new ManagerDashboardViewModel
    {
      TotalOrdersThisMonth = totalOrdersThisMonth,
      TotalSalesThisMonth = (decimal)totalSalesThisMonth,
      TaxCollectedThisYear = taxCollectedThisYear,
      SalesForecast = salesForecast,
      ProductSalesTrends = productSalesTrends,
      EmployeeTargetStatuses = employeeTargetStatuses,
      OutOfStockItems = outOfStockItems
    };

    return View(viewModel);
  }

  private async Task<List<SalesForecast>> GenerateSalesForecast()
  {
    var oneYearAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-1));

    var orderData = await _context.Orders
        .Where(o => o.OrderDate > oneYearAgo)
        .Select(o => new
        {
          o.OrderDate.Year,
          o.OrderDate.Month,
          Sales = o.ProductsOrders.Sum(po => po.Qty * po.Product.Price)
        })
        .ToListAsync();

    var forecastData = orderData
        .GroupBy(o => new { o.Year, o.Month })
        .Select(g => new
        {
          Year = g.Key.Year,
          Month = g.Key.Month,
          TotalSales = g.Sum(x => x.Sales)
        })
        .OrderBy(f => f.Year)
        .ThenBy(f => f.Month)
        .ToList();

    var forecast = forecastData
        .Select(f => new SalesForecast
        {
          Month = f.Year + "-" + f.Month,
          PredictedSales = (decimal)f.TotalSales
        })
        .ToList();

    return forecast;
  }

  private async Task<List<ProductSalesTrend>> GetProductSalesTrends()
  {
    var oneYearAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-1));
    var productSalesTrends = await _context.Products
        .Select(p => new ProductSalesTrend
        {
          ProductName = p.Pname,
          MonthlySales = _context.Orders
                .Where(o => o.OrderDate > oneYearAgo)
                .SelectMany(o => o.ProductsOrders)
                .Where(po => po.ProductId == p.ProductId)
                .GroupBy(po => new { po.Order.OrderDate.Year, po.Order.OrderDate.Month })
                .Select(g => new MonthlySales
                {
                  Month = g.Key.Year + "-" + g.Key.Month,
                  Quantity = g.Sum(po => po.Qty),
                  TotalSales = (decimal)g.Sum(po => po.Qty * po.Product.Price)
                })
                .OrderBy(m => m.Month)
                .ToList()
        })
        .ToListAsync();

    return productSalesTrends;
  }

  private async Task<List<EmployeeTargetStatus>> GetEmployeeTargetStatuses()
  {
    var employees = await _context.Employees.ToListAsync();
    var employeeTargetStatuses = new List<EmployeeTargetStatus>();

    foreach (var employee in employees)
    {
      var orders = await _context.Orders
          .Where(o => o.EmployeeId == employee.EmployeeId && o.OrderDate.Year == DateTime.Now.Year)
          .ToListAsync();

      var totalSales = orders
          .SelectMany(o => o.ProductsOrders)
          .Sum(po => po.Qty * po.Product.Price);

      var hasReachedTarget = totalSales >= employee.ETarget;

      employeeTargetStatuses.Add(new EmployeeTargetStatus
      {
        EmployeeName = $"{employee.EFirstname} {employee.ELastname}",
        HasReachedTarget = hasReachedTarget,
        Sales = (decimal)totalSales,
        Target = employee.ETarget
      });
    }

    return employeeTargetStatuses;
  }



  public async Task<IActionResult> SalesDashboard()
  {
    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
    if (string.IsNullOrEmpty(userEmail))
    {
      return View("Index");
    }

    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EEmail == userEmail);
    if (employee == null)
    {
      return View("Index");
    }

    var currentMonth = DateTime.Now.Month;
    var currentYear = DateTime.Now.Year;

    var ordersThisMonth = await _context.Orders
        .Include(o => o.ProductsOrders)
        .ThenInclude(po => po.Product)
        .Where(o => o.EmployeeId == employee.EmployeeId && o.OrderDate.Year == currentYear && o.OrderDate.Month == currentMonth)
        .ToListAsync();

    var ordersCount = ordersThisMonth.Count;
    var totalSalesThisMonth = ordersThisMonth
        .SelectMany(o => o.ProductsOrders)
        .Sum(po => po.Qty * po.Product.Price);

    var predictedCommission = totalSalesThisMonth * (double)(employee.EComissionPerc / 100m);

    var monthlyTarget = employee.ETarget;
    var hasReachedTarget = totalSalesThisMonth >= monthlyTarget;

    string encouragingMessage = hasReachedTarget
        ? "Congratulations! You have reached your target!"
        : "Keep going! You are almost there!";

    var pastYearOrders = await _context.Orders
        .Where(o => o.EmployeeId == employee.EmployeeId && o.OrderDate > DateOnly.FromDateTime(DateTime.Now.AddYears(-1)))
        .ToListAsync();

    var pastYearData = pastYearOrders
        .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
        .Select(g => new MonthlyOverview
        {
          Month = g.Key.Year + "-" + g.Key.Month,
          Orders = g.Count(),
          TotalSales = (decimal)g.Sum(o => o.ProductsOrders.Sum(po => po.Qty * po.Product.Price))
        })
        .OrderBy(m => m.Month)
        .ToList();

    var viewModel = new SalesRepDashboardViewModel
    {
      OrdersThisMonth = ordersCount,
      PredictedCommission = (decimal)predictedCommission,
      HasReachedTarget = hasReachedTarget,
      MonthlyTarget = monthlyTarget,
      SalesRepName = $"{employee.EFirstname} {employee.ELastname}",
      EncouragingMessage = encouragingMessage,
      MonthlyOverviews = pastYearData,
      TotalSalesThisMonth = (decimal)totalSalesThisMonth
    };

    return View(viewModel);
  }




  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
