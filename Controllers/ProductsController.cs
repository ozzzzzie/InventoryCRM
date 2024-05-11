using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
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
    //public async Task<IActionResult> Index()
    //{
    //    var naimsdbContext = _context.Products.Include(p => p.Brand);
    //    return View(await naimsdbContext.ToListAsync());
    //}
    public async Task<IActionResult> Index(string sortOrder)
    {
      //bug
      //var brands = _context.Brands.ToList();
      //ViewBag.Brands = new SelectList(brands, "BrandId", "Bname");

      // sorting data
      ViewData["BarcodeSortParm"] = sortOrder == "Barcode" ? "Barcode_desc" : "Barcode";
      ViewData["SKUSortParm"] = sortOrder == "SKU" ? "SKU_desc" : "SKU";
      ViewData["BrandSortParm"] = String.IsNullOrEmpty(sortOrder) ? "brand_desc" : "Brand";
      ViewData["ProdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "prod_desc" : "Prods";
      ViewData["SizeSortParm"] = sortOrder == "Size" ? "Size_desc" : "Size";
      ViewData["WQSortParm"] = sortOrder == "Warehouse Quantity" ? "WQTY_desc" : "Warehouse Quantity";
      ViewData["WQStatSortParm"] = sortOrder == "Warehouse Status" ? "WSTS_desc" : "Warehouse Status";
      ViewData["LclSortParm"] = sortOrder == "Local Quantity" ? "LCLQTY_desc" : "Local Quantity";
      ViewData["LclStatSortParm"] = sortOrder == "Local Status" ? "LCLSTS_desc" : "Local Status";

      var products = from p in _context.Products
                     select p;

      switch (sortOrder)
      {
        case "Barcode":
          products = products.OrderByDescending(p => p.Barcode).Reverse();
          break;
        case "Barcode_desc":
          products = products.OrderByDescending(p => p.Barcode);
          break;
        case "SKU":
          products = products.OrderByDescending(p => p.Sku).Reverse();
          break;
        case "SKU_desc":
          products = products.OrderByDescending(p => p.Sku);
          break;
        //check on this again - bug with desc for brand and names
        case "brand_desc":
          products = products.OrderByDescending(p => p.BrandId);
          break;
        case "Brand":
          products = products.OrderByDescending(p => p.BrandId).Reverse();
          break;
        case "prod_desc":
          products = products.OrderByDescending(p => p.Pname);
          break;
        case "Prods":
          products = products.OrderByDescending(p => p.Pname).Reverse();
          break;
        case "Size":
          products = products.OrderByDescending(p => p.Size).Reverse();
          break;
        case "Size_desc":
          products = products.OrderByDescending(p => p.Size);
          break;
        case "Warehouse Quantity":
          products = products.OrderByDescending(p => p.WarehouseQty).Reverse();
          break;
        case "WQTY_desc":
          products = products.OrderByDescending(p => p.WarehouseQty);
          break;
        case "WSTS_desc":
          products = products.OrderByDescending(p => p.WarehouseStatus);
          break;
        case "Warehouse Status":
          products = products.OrderByDescending(p => p.WarehouseStatus).Reverse();
          break;
        case "Local Quantity":
          products = products.OrderByDescending(p => p.LocalQty).Reverse();
          break;
        case "LCLQTY_desc":
          products = products.OrderByDescending(p => p.LocalQty);
          break;
        case "LCLSTS_desc":
          products = products.OrderByDescending(p => p.LocalStatus);
          break;
        case "Local Status":
          products = products.OrderByDescending(p => p.LocalStatus).Reverse();
          break;
        default:
          products = products.OrderBy(p => p.Barcode);
          break;
      }

      return View(await products.AsNoTracking().ToListAsync());
    }

    public async Task<IActionResult> LocalInventory(string sortOrder)
    {

      ViewData["BarcodeSortParm"] = sortOrder == "Barcode" ? "Barcode_desc" : "Barcode";
      ViewData["SKUSortParm"] = sortOrder == "SKU" ? "SKU_desc" : "SKU";
      ViewData["BrandSortParm"] = String.IsNullOrEmpty(sortOrder) ? "brand_desc" : "Brand";
      ViewData["ProdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "prod_desc" : "Prods";
      ViewData["SizeSortParm"] = sortOrder == "Size" ? "Size_desc" : "Size";
      ViewData["LclSortParm"] = sortOrder == "Local Quantity" ? "LCLQTY_desc" : "Local Quantity";
      ViewData["LclStatSortParm"] = sortOrder == "Local Status" ? "LCLSTS_desc" : "Local Status";



      var products = from p in _context.Products
                     select p;
      switch (sortOrder)
      {
        case "Barcode":
          products = products.OrderByDescending(p => p.Barcode).Reverse();
          break;
        case "Barcode_desc":
          products = products.OrderByDescending(p => p.Barcode);
          break;
        case "SKU":
          products = products.OrderByDescending(p => p.Sku).Reverse();
          break;
        case "SKU_desc":
          products = products.OrderByDescending(p => p.Sku);
          break;
        //check on this again - bug with desc for brand and names
        case "brand_desc":
          products = products.OrderByDescending(p => p.BrandId);
          break;
        case "Brand":
          products = products.OrderByDescending(p => p.BrandId).Reverse();
          break;
        case "prod_desc":
          products = products.OrderByDescending(p => p.Pname);
          break;
        case "Prods":
          products = products.OrderByDescending(p => p.Pname).Reverse();
          break;
        case "Size":
          products = products.OrderByDescending(p => p.Size).Reverse();
          break;
        case "Size_desc":
          products = products.OrderByDescending(p => p.Size);
          break;
        case "Local Quantity":
          products = products.OrderByDescending(p => p.LocalQty).Reverse();
          break;
        case "LCLQTY_desc":
          products = products.OrderByDescending(p => p.LocalQty);
          break;
        case "LCLSTS_desc":
          products = products.OrderByDescending(p => p.LocalStatus);
          break;
        case "Local Status":
          products = products.OrderByDescending(p => p.LocalStatus).Reverse();
          break;
        default:
          products = products.OrderBy(p => p.Barcode);
          break;
      }

      return View(await products.AsNoTracking().ToListAsync());
    }

    public async Task<IActionResult> WarehouseInventory(string sortOrder)
    {

      ViewData["BarcodeSortParm"] = sortOrder == "Barcode" ? "Barcode_desc" : "Barcode";
      ViewData["SKUSortParm"] = sortOrder == "SKU" ? "SKU_desc" : "SKU";
      ViewData["BrandSortParm"] = String.IsNullOrEmpty(sortOrder) ? "brand_desc" : "Brand";
      ViewData["ProdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "prod_desc" : "Prods";
      ViewData["SizeSortParm"] = sortOrder == "Size" ? "Size_desc" : "Size";
      ViewData["WQSortParm"] = sortOrder == "Warehouse Quantity" ? "WQTY_desc" : "Warehouse Quantity";
      ViewData["WQStatSortParm"] = sortOrder == "Warehouse Status" ? "WSTS_desc" : "Warehouse Status";

      var products = from p in _context.Products
                     select p;
      switch (sortOrder)
      {
        case "Barcode":
          products = products.OrderByDescending(p => p.Barcode).Reverse();
          break;
        case "Barcode_desc":
          products = products.OrderByDescending(p => p.Barcode);
          break;
        case "SKU":
          products = products.OrderByDescending(p => p.Sku).Reverse();
          break;
        case "SKU_desc":
          products = products.OrderByDescending(p => p.Sku);
          break;
        //check on this again - bug with desc for brand and names
        case "Brand":
          products = products.OrderByDescending(p => p.BrandId).Reverse();
          break;
        case "brand_desc":
          products = products.OrderByDescending(p => p.BrandId);
          break;
        case "prod_desc":
          products = products.OrderByDescending(p => p.Pname);
          break;
        case "Prods":
          products = products.OrderByDescending(p => p.Pname).Reverse();
          break;
        case "Size":
          products = products.OrderByDescending(p => p.Size).Reverse();
          break;
        case "Size_desc":
          products = products.OrderByDescending(p => p.Size);
          break;
        case "Warehouse Quantity":
          products = products.OrderByDescending(p => p.WarehouseQty).Reverse();
          break;
        case "WQTY_desc":
          products = products.OrderByDescending(p => p.WarehouseQty);
          break;
        case "WSTS_desc":
          products = products.OrderByDescending(p => p.WarehouseStatus);
          break;
        case "Warehouse Status":
          products = products.OrderByDescending(p => p.WarehouseStatus).Reverse();
          break;
        default:
          products = products.OrderBy(p => p.Barcode);
          break;
      }

      return View(await products.AsNoTracking().ToListAsync());
    }
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


        //var qty = moveQuantities[id];

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
