using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NAIMS.Models;

namespace NAIMS.Controllers
{
  public class ProductsController : Controller
  {
    private readonly NaimsdbContext _context;

    public ProductsController(NaimsdbContext context)
    {
      _context = context;
    }

    // GET: Products
    [Authorize]
    public async Task<IActionResult> Index(string sortOrder)
    {
      var products = from p in _context.Products
                     select p;

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> LocalInventory(string sortOrder)
    {
      var products = from p in _context.Products
                     select p;

      return View(products);
    }
    [Authorize]
    public async Task<IActionResult> WarehouseInventory()
    {
      var products = from p in _context.Products
                     select p;

      return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var product = await _context.Products
          .Include(p => p.Brand)
          .FirstOrDefaultAsync(m => m.ProductId == id);
      if (product == null)
      {
        return NotFound();
      }

      return View(product);
    }

    [Authorize]
    public IActionResult MovingItems()
    {
      var products = _context.Products.ToList();
      var brands = _context.Brands.ToList();

      ViewBag.Brands = new SelectList(brands, "BrandId", "Bname");
      return View(products);
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> ProcessMoveItems(
  List<int> productIds,
  Dictionary<int, int> moveQuantities,
  string direction)
    {

      if (productIds.Count != moveQuantities.Count)
      {
        return BadRequest("Quantities don't match products");
      }

      foreach (var id in productIds)
      {

        var qty = moveQuantities[id];
        var product = await _context.Products.FindAsync(id);

        int warehouseQty = product.WarehouseQty;
        int localQty = product.LocalQty;

        if (direction == "warehouseToLocal")
        {
          product.WarehouseQty -= qty;
          product.LocalQty += qty;
        }
        else if (direction == "localToWarehouse")
        {
          product.WarehouseQty += qty;
          product.LocalQty -= qty;
        }

        // Update warehouse status

        if (product.WarehouseQty == 0)
        {
          product.WarehouseStatus = "out of stock";
        }
        else if (product.WarehouseQty <= 24)
        {
          product.WarehouseStatus = "low stock";
        }
        else
        {
          product.WarehouseStatus = "in stock";
        }

        // Update local status

        if (product.LocalQty == 0)
        {
          product.LocalStatus = "pick up needed";
        }
        else if (product.LocalQty <= 24)
        {
          product.LocalStatus = "low stock";
        }
        else
        {
          product.LocalStatus = "in stock";
        }

      }

      await _context.SaveChangesAsync();

      return RedirectToAction("Index");

    }

    [Authorize(Roles = "SuperAdmin,Manager")]
    // GET: Products1/Create
    public IActionResult Create()
    {
      ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Bname");
      return View();
    }

    // POST: Products1/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductId,Barcode,Sku,BrandId,Pname,Size,Pdescription,Price,PriceVat,WarehouseQty,WarehouseStatus,LocalQty,LocalStatus")] Product product)
    {
      if (ModelState.IsValid)
      {
        _context.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Bname", product.BrandId);
      return View(product);
    }

    [Authorize(Roles = "SuperAdmin,Manager")]
    // GET: Products1/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var product = await _context.Products.FindAsync(id);
      if (product == null)
      {
        return NotFound();
      }
      ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Bname", product.BrandId);
      return View(product);
    }

    // POST: Products1/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProductId,Barcode,Sku,BrandId,Pname,Size,Pdescription,Price,PriceVat,WarehouseQty,WarehouseStatus,LocalQty,LocalStatus")] Product product)
    {
      if (id != product.ProductId)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(product);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ProductExists(product.ProductId))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Bname", product.BrandId);
      return View(product);
    }
    [Authorize(Roles = "SuperAdmin,Manager")]
    // GET: Products1/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var product = await _context.Products
          .Include(p => p.Brand)
          .FirstOrDefaultAsync(m => m.ProductId == id);
      if (product == null)
      {
        return NotFound();
      }

      return View(product);
    }

    // POST: Products1/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var product = await _context.Products.FindAsync(id);
      if (product != null)
      {
        _context.Products.Remove(product);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
      return _context.Products.Any(e => e.ProductId == id);
    }
  }
}
