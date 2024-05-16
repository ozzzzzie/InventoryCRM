using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NAIMS.Models;

namespace NAIMS.Controllers
{
  public class OrdersController : Controller
  {
    private readonly NaimsdbContext _context;

    public OrdersController(NaimsdbContext context)
    {
      _context = context;
    }

    public IActionResult AddProducts()
    {
      PopulateViewBags();

      var products = _context.Products
          .Include(p => p.Brand)
          .ToList();

      var productSelectList = products.Select(p => new SelectListItem
      {
        Value = p.ProductId.ToString(),
        Text = (p.Brand != null ? p.Brand.Bname + " - " : "") + p.Pname + (p.Size != null ? " (" + p.Size + ")" : "")
      }).ToList();

      var productPrices = products.ToDictionary(p => p.ProductId, p => p.Price);

      var viewModel = new AddProductsViewModel
      {
        Products = productSelectList,
        ProductPrices = productPrices,
        Order = new Order(),
        ProductsOrders = new List<ProductsOrder> { new ProductsOrder() }
      };

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AddProducts(AddProductsViewModel viewModel)
    {
      // Manually remove validation for Products and ProductPrices
      ModelState.Remove("Products");
      ModelState.Remove("ProductPrices");

      if (ModelState.IsValid)
      {
        viewModel.Order.OrderDate = DateOnly.FromDateTime(DateTime.Now); // Set the order date to the current date

        _context.Add(viewModel.Order);
        await _context.SaveChangesAsync();

        foreach (var productsOrder in viewModel.ProductsOrders)
        {
          productsOrder.OrderId = viewModel.Order.OrderId;
          _context.Add(productsOrder);

          var product = await _context.Products.FindAsync(productsOrder.ProductId);
          product.LocalQty -= productsOrder.Qty;
          _context.Products.Update(product);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
      }

      // Re-populate the Products and ProductPrices properties in case of validation failure
      PopulateViewBags();

      var products = _context.Products
          .Include(p => p.Brand)
          .ToList();

      viewModel.Products = products.Select(p => new SelectListItem
      {
        Value = p.ProductId.ToString(),
        Text = (p.Brand != null ? p.Brand.Bname + " - " : "") + p.Pname + (p.Size != null ? " (" + p.Size + ")" : "")
      }).ToList();

      viewModel.ProductPrices = products.ToDictionary(p => p.ProductId, p => p.Price);

      return View(viewModel);
    }

    private void PopulateViewBags()
    {
      var emp = _context.Employees.Select(e => new
      {
        e.EmployeeId,
        FullName = $"{e.EFirstname} {e.ELastname}"
      });

      ViewData["EmployeeId"] = new SelectList(emp, "EmployeeId", "FullName");
      ViewData["ContactId"] = new SelectList(_context.Contacts, "ContactId", "Cname");
    }

    // GET: Orders
    public async Task<IActionResult> Index()
    {
      var naimsdbContext = _context.Orders.Include(o => o.Contact).Include(o => o.Employee);
      return View(await naimsdbContext.ToListAsync());
    }

    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var order = await _context.Orders
          .Include(o => o.Contact)
          .Include(o => o.Employee)
          .FirstOrDefaultAsync(m => m.OrderId == id);
      if (order == null)
      {
        return NotFound();
      }

      return View(order);
    }

    // GET: Orders/Create
    public IActionResult Create()
    {

      var emp = _context.Employees.Select(e => new
      {
        e.EmployeeId,
        FullName = $"{e.EFirstname} {e.ELastname}"
      });

      ViewData["EmployeeId"] = new SelectList(emp, "EmployeeId", "FullName");
      ViewData["ContactId"] = new SelectList(_context.Contacts, "ContactId", "Cname");

      return View();

    }

    // POST: Orders/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OrderId,ContactId,OrderDate,EmployeeId")] Order order)
    {
      if (ModelState.IsValid)
      {
        _context.Add(order);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["ContactId"] = new SelectList(_context.Contacts, "ContactId", "ContactId", order.ContactId);
      ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", order.EmployeeId);
      return View(order);
    }

    // GET: Orders/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var order = await _context.Orders.FindAsync(id);
      if (order == null)
      {
        return NotFound();
      }
      ViewData["ContactId"] = new SelectList(_context.Contacts, "ContactId", "ContactId", order.ContactId);
      ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", order.EmployeeId);
      return View(order);
    }

    // POST: Orders/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("OrderId,ContactId,OrderDate,EmployeeId")] Order order)
    {
      if (id != order.OrderId)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(order);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!OrderExists(order.OrderId))
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
      ViewData["ContactId"] = new SelectList(_context.Contacts, "ContactId", "ContactId", order.ContactId);
      ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", order.EmployeeId);
      return View(order);
    }

    // GET: Orders/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var order = await _context.Orders
          .Include(o => o.Contact)
          .Include(o => o.Employee)
          .FirstOrDefaultAsync(m => m.OrderId == id);
      if (order == null)
      {
        return NotFound();
      }

      return View(order);
    }

    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var order = await _context.Orders.FindAsync(id);
      if (order != null)
      {
        _context.Orders.Remove(order);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool OrderExists(int id)
    {
      return _context.Orders.Any(e => e.OrderId == id);
    }
  }
}
