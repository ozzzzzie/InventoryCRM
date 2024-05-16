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
      var emp = _context.Employees.Select(e => new
      {
        e.EmployeeId,
        FullName = $"{e.EFirstname} {e.ELastname}"
      });

      ViewData["EmployeeId"] = new SelectList(emp, "EmployeeId", "FullName");
      ViewData["ContactId"] = new SelectList(_context.Contacts, "ContactId", "Cname");

      var viewModel = new AddProductsViewModel
      {
        Products = _context.Products.ToList(),
        //Orders = _context.Orders.ToList(),
        Order = new Order(),
        ProductsOrder = new ProductsOrder()
      };

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AddProducts(AddProductsViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        _context.Add(viewModel.Order);
        await _context.SaveChangesAsync();

        viewModel.ProductsOrder.OrderId = viewModel.Order.OrderId;

        _context.Add(viewModel.ProductsOrder);

        var product = await _context.Products.FindAsync(viewModel.ProductsOrder.ProductId);

        product.LocalQty -= viewModel.ProductsOrder.Qty;

        _context.Products.Update(product);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
      }

      var emp = _context.Employees.Select(e => new
      {
        e.EmployeeId,
        FullName = $"{e.EFirstname} {e.ELastname}"
      });

      ViewData["EmployeeId"] = new SelectList(emp, "EmployeeId", "FullName");
      ViewData["ContactId"] = new SelectList(_context.Contacts, "ContactId", "Cname");

      viewModel.Products = _context.Products.ToList();
      //viewModel.Orders = _context.Orders.ToList();

      return View(viewModel);
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
