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

    // GET: Orders/AddProducts
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
    public async Task<IActionResult> AddProducts(AddProductsViewModel viewModel)
    {
      // Manually remove validation for Products and ProductPrices
      ModelState.Remove("Products");
      ModelState.Remove("ProductPrices");

      // Manually remove validation for navigation properties in ProductsOrders
      foreach (var productsOrder in viewModel.ProductsOrders)
      {
        ModelState.Remove($"ProductsOrders[{viewModel.ProductsOrders.IndexOf(productsOrder)}].Order");
        ModelState.Remove($"ProductsOrders[{viewModel.ProductsOrders.IndexOf(productsOrder)}].Product");
      }

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
      var orders = await _context.Orders
          .Include(o => o.Contact)
          .Include(o => o.Employee)
          .ToListAsync();
      return View(orders);
    }

    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int id)
    {
      var order = await _context.Orders
          .Include(o => o.Contact)
          .Include(o => o.Employee)
          .Include(o => o.ProductsOrders)
              .ThenInclude(po => po.Product)
                  .ThenInclude(p => p.Brand)
          .FirstOrDefaultAsync(o => o.OrderId == id);

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
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var order = await _context.Orders
          .Include(o => o.ProductsOrders)
          .FirstOrDefaultAsync(o => o.OrderId == id);

      if (order == null)
      {
        return NotFound();
      }

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
        Order = order,
        Products = productSelectList,
        ProductPrices = productPrices,
        ProductsOrders = order.ProductsOrders.ToList()
      };

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AddProductsViewModel viewModel)
    {
      if (id != viewModel.Order.OrderId)
      {
        return NotFound();
      }

      // Manually remove validation for Products and ProductPrices
      ModelState.Remove("Products");
      ModelState.Remove("ProductPrices");

      // Manually remove validation for navigation properties in ProductsOrders
      foreach (var productsOrder in viewModel.ProductsOrders)
      {
        ModelState.Remove($"ProductsOrders[{viewModel.ProductsOrders.IndexOf(productsOrder)}].Order");
        ModelState.Remove($"ProductsOrders[{viewModel.ProductsOrders.IndexOf(productsOrder)}].Product");
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(viewModel.Order);
          await _context.SaveChangesAsync();

          foreach (var productsOrder in viewModel.ProductsOrders)
          {
            var existingProductOrder = await _context.ProductsOrders
                .FirstOrDefaultAsync(po => po.ProductorderId == productsOrder.ProductorderId);

            if (existingProductOrder != null)
            {
              existingProductOrder.Qty = productsOrder.Qty;
              _context.Update(existingProductOrder);
            }
            else
            {
              productsOrder.OrderId = viewModel.Order.OrderId;
              _context.Add(productsOrder);
            }

            var product = await _context.Products.FindAsync(productsOrder.ProductId);
            product.LocalQty -= productsOrder.Qty;
            _context.Products.Update(product);
          }

          if (viewModel.ProductsOrdersToRemove != null)
          {
            foreach (var productOrderId in viewModel.ProductsOrdersToRemove)
            {
              var productOrder = await _context.ProductsOrders.FindAsync(productOrderId);
              if (productOrder != null)
              {
                _context.ProductsOrders.Remove(productOrder);

                var product = await _context.Products.FindAsync(productOrder.ProductId);
                if (product != null)
                {
                  product.LocalQty += productOrder.Qty;
                  _context.Products.Update(product);
                }
              }
            }
          }

          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!OrderExists(viewModel.Order.OrderId))
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

    private bool OrderExists(int id)
    {
      return _context.Orders.Any(e => e.OrderId == id);
    }


    // GET: Orders/Delete/5
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
          .Include(o => o.ProductsOrders)
              .ThenInclude(po => po.Product)
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
      var order = await _context.Orders
          .Include(o => o.ProductsOrders)
          .FirstOrDefaultAsync(o => o.OrderId == id);

      if (order != null)
      {
        foreach (var productsOrder in order.ProductsOrders)
        {
          var product = await _context.Products.FindAsync(productsOrder.ProductId);
          product.LocalQty += productsOrder.Qty;
          _context.Products.Update(product);
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
      }

      return RedirectToAction(nameof(Index));
    }

  }
}
