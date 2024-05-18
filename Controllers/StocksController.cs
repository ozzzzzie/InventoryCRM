using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAIMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace NAIMS.Controllers
{
  public class StocksController : Controller
  {
    private readonly NaimsdbContext _context;

    public StocksController(NaimsdbContext context)
    {
      _context = context;
    }
    [Authorize]
    public async Task<IActionResult> WarehouseStockReport()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> WarehouseStockReportPrint()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> LocalStockReport()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> LocalStockReportPrint()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> WarehouseOutOfStockItems()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> WarehouseOutOfStockItemsPrint()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> WarehouseLowStockItems()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> WarehouseLowStockItemsPrint()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> WarehouseInStockItems()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> WarehouseInStockItemsPrint()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> LocalPickUpItems()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> LocalPickUpItemsPrint()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> LocalLowStockItems()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }

    [Authorize]
    public async Task<IActionResult> LocalInStockItems()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }

    [Authorize]
    public async Task<IActionResult> LocalLowStockItemsPrint()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }


    [Authorize]
    public async Task<IActionResult> LocalInStockItemsPrint()
    {
      var products = await _context.Products
          .Include(p => p.Brand)
          .ToListAsync();

      return View(products);
    }
  }
}
