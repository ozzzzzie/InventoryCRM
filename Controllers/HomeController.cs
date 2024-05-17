using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcStarter.Models;
using Microsoft.AspNetCore.Authorization;
using NAIMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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

  public IActionResult ManagerDashboard()
  {
    return View();
  }

  public async Task<IActionResult> SalesDashboard()
  {
    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
    if (string.IsNullOrEmpty(userEmail))
    {
      return Unauthorized("User is not authorized.");
    }

    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EEmail == userEmail);
    if (employee == null)
    {
      return NotFound("Sales representative not found.");
    }

    var currentMonth = DateTime.Now.Month;
    var currentYear = DateTime.Now.Year;

    var ordersThisMonth = await _context.Orders
        .Include(o => o.ProductsOrders)
        .ThenInclude(po => po.Product)
        .Where(o => o.EmployeeId == employee.EmployeeId && o.OrderDate.Year == currentYear && o.OrderDate.Month == currentMonth)
        .ToListAsync();

    var ordersCount = ordersThisMonth.Count;
    var predictedCommission = ordersThisMonth
        .SelectMany(o => o.ProductsOrders)
        .Sum(po => po.Qty * po.Product.Price * (double)(employee.EComissionPerc / 100m));

    var monthlyTarget = employee.ETarget;
    var hasReachedTarget = predictedCommission >= monthlyTarget;

    string encouragingMessage = hasReachedTarget
        ? "Congratulations! You have reached your target!"
        : "Keep going! You are almost there!";

    // Retrieve past year data
    var pastYearOrders = await _context.Orders
        .Where(o => o.EmployeeId == employee.EmployeeId && o.OrderDate > DateOnly.FromDateTime(DateTime.Now.AddYears(-1)))
        .ToListAsync();

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
        .ToList();

    var viewModel = new SalesRepDashboardViewModel
    {
      OrdersThisMonth = ordersCount,
      PredictedCommission = (decimal)predictedCommission,
      HasReachedTarget = hasReachedTarget,
      MonthlyTarget = monthlyTarget,
      SalesRepName = $"{employee.EFirstname} {employee.ELastname}",
      EncouragingMessage = encouragingMessage,
      MonthlyOverviews = pastYearData
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
