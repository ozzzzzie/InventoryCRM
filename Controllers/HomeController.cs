using System.Diagnostics; 
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcStarter.Models;
using Microsoft.AspNetCore.Authorization;
using NAIMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspnetCoreMvcStarter.Controllers
{
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
        // if the user's role is undefined, redirect them to the main page
        return View();
      }
    }

    public async Task<IActionResult> SuperAdminDashboard()
    {
      var currentMonth = DateTime.Now.Month; // getting the current month
      var currentYear = DateTime.Now.Year; // getting the current year

      var ordersThisMonth = await _context.Orders
          .Include(o => o.ProductsOrders)
          .ThenInclude(po => po.Product)
          .Where(o => o.OrderDate.Year == currentYear && o.OrderDate.Month == currentMonth)
          .ToListAsync(); // querying the orders for the current month

      var totalOrdersThisMonth = ordersThisMonth.Count; // calculating the total number of orders this month
      var totalSalesThisMonth = ordersThisMonth.Sum(o => o.ProductsOrders.Sum(po => po.Qty * po.Product.Price)); // calculating the total sales this month
      var totalTaxThisYear = await CalculateTotalTaxThisYear(currentYear); // calculating the total tax for this year

      var pastYearData = await GetMonthlyOverviewForPastYear(); // getting the monthly overview data for the past year
      var aSalesForecast = await GenerateASalesForecast(); // generating the sales forecast
      var employeePerformances = await GetAEmployeePerformances(); // getting the employee performance data

      var viewModel = new SuperAdminDashboardViewModel
      {
        TotalOrders = totalOrdersThisMonth,
        TotalSales = (decimal)totalSalesThisMonth,
        TotalTax = totalTaxThisYear,
        MonthlyOverviews = pastYearData,
        ASalesForecasts = aSalesForecast,
        EmployeePerformances = employeePerformances // populating the view model with the calculated data
      };

      return View(viewModel); // returning the view with the view model
    }

    private async Task<decimal> CalculateTotalTaxThisYear(int currentYear)
    {
      var ordersThisYear = await _context.Orders
          .Include(o => o.ProductsOrders)
          .ThenInclude(po => po.Product)
          .Where(o => o.OrderDate.Year == currentYear)
          .ToListAsync(); // querying the orders for the current year

      var totalTaxThisYear = ordersThisYear.Sum(o => o.ProductsOrders.Sum(po => po.Qty * (int)po.Product.Price * 0.1m)); // calculating the total tax for this year

      return totalTaxThisYear; // returning the total tax
    }

    private async Task<List<AMonthlyOverview>> GetMonthlyOverviewForPastYear()
    {
      var currentDate = DateTime.Now; // getting the current date
      var pastYearDate = currentDate.AddYears(-1); // calculating the date one year ago

      var pastYearOrders = await _context.Orders
          .Include(o => o.ProductsOrders)
          .ThenInclude(po => po.Product)
          .Where(o => (o.OrderDate.Year > pastYearDate.Year) ||
                      (o.OrderDate.Year == pastYearDate.Year && o.OrderDate.Month > pastYearDate.Month) ||
                      (o.OrderDate.Year == pastYearDate.Year && o.OrderDate.Month == pastYearDate.Month && o.OrderDate.Day >= pastYearDate.Day))
          .ToListAsync(); // querying the orders for the past year

      var groupedData = pastYearOrders
          .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
          .Select(g => new AMonthlyOverview
          {
            Month = g.Key.Year + "-" + g.Key.Month,
            Orders = g.Count(),
            TotalSales = (decimal)g.Sum(o => o.ProductsOrders.Sum(po => po.Qty * po.Product.Price))
          })
          .OrderBy(m => m.Month)
          .ToList(); // grouping the data by month and calculating the total sales for each month

      return groupedData; // returning the grouped data
    }

    private async Task<List<ASalesForecast>> GenerateASalesForecast()
    {
      var currentDate = DateTime.Now; // getting the current date
      var pastYearDate = currentDate.AddYears(-1); // calculating the date one year ago

      var orders = await _context.Orders
          .Include(o => o.ProductsOrders)
          .ThenInclude(po => po.Product)
          .Where(o => (o.OrderDate.Year > pastYearDate.Year) ||
                      (o.OrderDate.Year == pastYearDate.Year && o.OrderDate.Month > pastYearDate.Month) ||
                      (o.OrderDate.Year == pastYearDate.Year && o.OrderDate.Month == pastYearDate.Month && o.OrderDate.Day >= pastYearDate.Day))
          .ToListAsync(); // querying the orders for the past year

      var forecastData = orders
          .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
          .Select(g => new ASalesForecast
          {
            Month = g.Key.Year + "-" + g.Key.Month,
            PredictedSales = (decimal)g.Sum(o => o.ProductsOrders.Sum(po => po.Qty * po.Product.Price))
          })
          .OrderBy(f => f.Month)
          .ToList(); // grouping the data by month and calculating the predicted sales for each month

      return forecastData; // returning the forecast data
    }

    private async Task<List<AEmployeePerformance>> GetAEmployeePerformances()
    {
      var employees = await _context.Employees.ToListAsync(); // querying the list of employees
      var employeePerformances = new List<AEmployeePerformance>(); // initializing the list of employee performances

      foreach (var employee in employees)
      {
        var orders = await _context.Orders
            .Include(o => o.ProductsOrders)
            .ThenInclude(po => po.Product)
            .Where(o => o.EmployeeId == employee.EmployeeId && o.OrderDate.Year == DateTime.Now.Year)
            .ToListAsync(); // querying the orders for the current year for each employee

        var totalSales = orders.Sum(o => o.ProductsOrders.Sum(po => po.Qty * po.Product.Price)); // calculating the total sales for each employee
        var hasReachedTarget = (totalSales >= employee.ETarget * 12); // checking if the employee has reached their target

        employeePerformances.Add(new AEmployeePerformance
        {
          EmployeeName = $"{employee.EFirstname} {employee.ELastname}",
          Sales = (decimal)totalSales,
          Target = employee.ETarget,
          HasReachedTarget = hasReachedTarget // adding the performance data to the list
        });
      }

      return employeePerformances; // returning the list of employee performances
    }

    public async Task<IActionResult> ManagerDashboard()
    {
      var currentMonth = DateTime.Now.Month; // getting the current month
      var currentYear = DateTime.Now.Year; // getting the current year

      var ordersThisMonth = await _context.Orders
          .Include(o => o.ProductsOrders)
          .ThenInclude(po => po.Product)
          .Where(o => o.OrderDate.Year == currentYear && o.OrderDate.Month == currentMonth)
          .ToListAsync(); // querying the orders for the current month

      var totalOrdersThisMonth = ordersThisMonth.Count; // calculating the total number of orders this month
      var totalSalesThisMonth = ordersThisMonth.Sum(o => o.ProductsOrders.Sum(po => po.Qty * po.Product.Price)); // calculating the total sales this month

      var allOrdersThisYear = await _context.Orders
          .Include(o => o.ProductsOrders)
          .ThenInclude(po => po.Product)
          .Where(o => o.OrderDate.Year == currentYear)
          .ToListAsync(); // querying all orders for the current year

      var taxCollectedThisYear = allOrdersThisYear.Sum(o => o.ProductsOrders.Sum(po => po.Qty * (int)po.Product.Price * 0.1m)); // calculating the tax collected this year

      var salesForecast = await GenerateSalesForecast(); // generating the sales forecast
      var productSalesTrends = await GetProductSalesTrends(); // getting the product sales trends
      var employeeTargetStatuses = await GetEmployeeTargetStatuses(); // getting the employee target statuses
      var outOfStockItems = await _context.Products.Where(p => p.WarehouseQty == 0).ToListAsync(); // querying the out of stock items

      var viewModel = new ManagerDashboardViewModel
      {
        TotalOrdersThisMonth = totalOrdersThisMonth,
        TotalSalesThisMonth = (decimal)totalSalesThisMonth,
        TaxCollectedThisYear = taxCollectedThisYear,
        SalesForecast = salesForecast,
        ProductSalesTrends = productSalesTrends,
        EmployeeTargetStatuses = employeeTargetStatuses,
        OutOfStockItems = outOfStockItems // populating the view model with the calculated data
      };

      return View(viewModel); // returning the view with the view model
    }

    private async Task<List<SalesForecast>> GenerateSalesForecast()
    {
      var oneYearAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)); // calculating the date one year ago

      var orderData = await _context.Orders
          .Where(o => o.OrderDate > oneYearAgo)
          .Select(o => new
          {
            o.OrderDate.Year,
            o.OrderDate.Month,
            Sales = o.ProductsOrders.Sum(po => po.Qty * po.Product.Price)
          })
          .ToListAsync(); // querying the order data for the past year

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
          .ToList(); // grouping the data by month and calculating the total sales for each month

      var forecast = forecastData
          .Select(f => new SalesForecast
          {
            Month = f.Year + "-" + f.Month,
            PredictedSales = (decimal)f.TotalSales
          })
          .ToList(); // creating the sales forecast objects

      return forecast; // returning the forecast data
    }

    private async Task<List<ProductSalesTrend>> GetProductSalesTrends()
    {
      var oneYearAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)); // calculating the date one year ago

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
          .ToListAsync(); // querying the product sales trends for the past year

      return productSalesTrends; // returning the product sales trends
    }

    private async Task<List<EmployeeTargetStatus>> GetEmployeeTargetStatuses()
    {
      var employees = await _context.Employees.ToListAsync(); // querying the list of employees
      var employeeTargetStatuses = new List<EmployeeTargetStatus>(); // initializing the list of employee target statuses

      foreach (var employee in employees)
      {
        var orders = await _context.Orders
            .Where(o => o.EmployeeId == employee.EmployeeId && o.OrderDate.Year == DateTime.Now.Year)
            .ToListAsync(); // querying the orders for the current year for each employee

        var totalSales = orders
            .SelectMany(o => o.ProductsOrders)
            .Sum(po => po.Qty * po.Product.Price); // calculating the total sales for each employee

        var hasReachedTarget = totalSales >= employee.ETarget; // checking if the employee has reached their target

        employeeTargetStatuses.Add(new EmployeeTargetStatus
        {
          EmployeeName = $"{employee.EFirstname} {employee.ELastname}",
          HasReachedTarget = hasReachedTarget,
          Sales = (decimal)totalSales,
          Target = employee.ETarget // adding the target status data to the list
        });
      }

      return employeeTargetStatuses; // returning the list of employee target statuses
    }

    public async Task<IActionResult> SalesDashboard()
    {
      var userEmail = User.FindFirst(ClaimTypes.Email)?.Value; // getting the user's email from the claims
      if (string.IsNullOrEmpty(userEmail))
      {
        return View("Index"); // if the email is null or empty, redirect to the index view
      }

      var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EEmail == userEmail); // querying the employee with the matching email
      if (employee == null)
      {
        return View("Index"); // if the employee is not found, redirect to the index view
      }

      var currentMonth = DateTime.Now.Month; // getting the current month
      var currentYear = DateTime.Now.Year; // getting the current year

      var ordersThisMonth = await _context.Orders
          .Include(o => o.ProductsOrders)
          .ThenInclude(po => po.Product)
          .Where(o => o.EmployeeId == employee.EmployeeId && o.OrderDate.Year == currentYear && o.OrderDate.Month == currentMonth)
          .ToListAsync(); // querying the orders for the current month for the employee

      var ordersCount = ordersThisMonth.Count; // calculating the number of orders this month
      var predictedCommission = ordersThisMonth
          .SelectMany(o => o.ProductsOrders)
          .Sum(po => po.Qty * po.Product.Price * (double)(employee.EComissionPerc / 100m)); // calculating the predicted commission for the employee

      var totalSalesThisMonth = ordersThisMonth
          .SelectMany(o => o.ProductsOrders)
          .Sum(po => po.Qty * po.Product.Price); // calculating the total sales this month

      var monthlyTarget = employee.ETarget; // getting the employee's monthly target
      var hasReachedTarget = totalSalesThisMonth >= monthlyTarget; // checking if the employee has reached their target

      string encouragingMessage = hasReachedTarget
          ? "Congratulations! You have reached your target!"
          : "Keep going! You are almost there!"; // generating an encouraging message based on the target status

      // Retrieve past year data
      var pastYearOrders = await _context.Orders
          .Include(o => o.ProductsOrders)
          .ThenInclude(po => po.Product)
          .Where(o => o.EmployeeId == employee.EmployeeId && o.OrderDate >= DateOnly.FromDateTime(DateTime.Now.AddYears(-1)))
          .ToListAsync(); // querying the orders for the past year for the employee

      // Group and calculate monthly overview
      var pastYearData = pastYearOrders
          .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
          .Select(g => new MonthlyOverview
          {
            Month = g.Key.Year + "-" + g.Key.Month,
            Orders = g.Count(),
            TotalSales = (decimal)g.Sum(o => o.ProductsOrders.Sum(po => po.Qty * po.Product.Price))
          })
          .OrderBy(m => m.Month)
          .ToList(); // grouping the data by month and calculating the total sales for each month

      var viewModel = new SalesRepDashboardViewModel
      {
        OrdersThisMonth = ordersCount,
        PredictedCommission = (decimal)predictedCommission,
        HasReachedTarget = hasReachedTarget,
        MonthlyTarget = monthlyTarget,
        SalesRepName = $"{employee.EFirstname} {employee.ELastname}",
        EncouragingMessage = encouragingMessage,
        MonthlyOverviews = pastYearData,
        TotalSalesThisMonth = (decimal)totalSalesThisMonth // populating the view model with the calculated data
      };

      return View(viewModel); // returning the view with the view model
    }

    private async Task<List<MonthlyOverview>> GetMonthlyOverviewForPastYear(int employeeId)
    {
      var oneYearAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)); // calculating the date one year ago
      var pastYearOrders = await _context.Orders
          .Include(o => o.ProductsOrders)
          .ThenInclude(po => po.Product)
          .Where(o => o.OrderDate >= oneYearAgo && o.EmployeeId == employeeId)
          .ToListAsync(); // querying the orders for the past year for the employee

      var pastYearData = pastYearOrders
          .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
          .Select(g => new MonthlyOverview
          {
            Month = g.Key.Year + "-" + g.Key.Month,
            Orders = g.Count(),
            TotalSales = g.Sum(o => o.ProductsOrders.Sum(po => po.Qty * (decimal)po.Product.Price))
          })
          .OrderBy(m => m.Month)
          .ToList(); // grouping the data by month and calculating the total sales for each month

      return pastYearData; // returning the grouped data
    }

    public IActionResult Privacy()
    {
      return View(); // returning the privacy view
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); // returning the error view with the error details
    }
  }
}
