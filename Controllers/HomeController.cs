using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcStarter.Models;
using Microsoft.AspNetCore.Authorization;
using NAIMS.Models;

namespace AspnetCoreMvcStarter.Controllers;

public class HomeController : Controller
{
  private readonly NaimsdbContext _context;

  public HomeController(NaimsdbContext context)
  {
    _context = context;
  }
  //private readonly ILogger<HomeController> _logger;

  //public HomeController(ILogger<HomeController> logger)
  //{
  //  _logger = logger;
  //}
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

  public IActionResult SalesDashboard()
  {
    return View();
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
