using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    // GET: Orders/AddProducts
    public IActionResult AddProducts()
    {
      PopulateViewBags(); // populate view bags for dropdown lists

      var products = _context.Products
          .Include(p => p.Brand)
          .ToList(); // query all products including their brand

      var productSelectList = products.Select(p => new SelectListItem
      {
        Value = p.ProductId.ToString(),
        Text = (p.Brand != null ? p.Brand.Bname + " - " : "") + p.Pname + (p.Size != null ? " (" + p.Size + ")" : "")
      }).ToList(); // create a select list for products

      var productPrices = products.ToDictionary(p => p.ProductId, p => p.Price); // create a dictionary for product prices

      var viewModel = new AddProductsViewModel
      {
        Products = productSelectList,
        ProductPrices = productPrices,
        Order = new Order(),
        ProductsOrders = new List<ProductsOrder> { new ProductsOrder() }
      }; // initialize the view model with products and order details

      return View(viewModel); // return the view with the view model
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProducts(AddProductsViewModel viewModel)
    {
      // manually remove validation for Products and ProductPrices
      ModelState.Remove("Products");
      ModelState.Remove("ProductPrices");

      // manually remove validation for navigation properties in ProductsOrders
      foreach (var productsOrder in viewModel.ProductsOrders)
      {
        ModelState.Remove($"ProductsOrders[{viewModel.ProductsOrders.IndexOf(productsOrder)}].Order");
        ModelState.Remove($"ProductsOrders[{viewModel.ProductsOrders.IndexOf(productsOrder)}].Product");
      }

      if (ModelState.IsValid)
      {
        _context.Add(viewModel.Order); // add the order to the context
        await _context.SaveChangesAsync(); // save the changes

        foreach (var productsOrder in viewModel.ProductsOrders)
        {
          productsOrder.OrderId = viewModel.Order.OrderId; // set the order id for the products order
          _context.Add(productsOrder); // add the products order to the context

          var product = await _context.Products.FindAsync(productsOrder.ProductId); // find the product
          product.LocalQty -= productsOrder.Qty; // reduce the local quantity of the product
          _context.Products.Update(product); // update the product
        }

        await _context.SaveChangesAsync(); // save the changes

        return RedirectToAction(nameof(Index)); // redirect to the index action
      }

      // re-populate the Products and ProductPrices properties in case of validation failure
      PopulateViewBags();

      var products = _context.Products
          .Include(p => p.Brand)
          .ToList(); // query all products including their brand

      viewModel.Products = products.Select(p => new SelectListItem
      {
        Value = p.ProductId.ToString(),
        Text = (p.Brand != null ? p.Brand.Bname + " - " : "") + p.Pname + (p.Size != null ? " (" + p.Size + ")" : "")
      }).ToList(); // create a select list for products

      viewModel.ProductPrices = products.ToDictionary(p => p.ProductId, p => p.Price); // create a dictionary for product prices

      return View(viewModel); // return the view with the view model
    }

    private void PopulateViewBags()
    {
      var emp = _context.Employees.Select(e => new
      {
        e.EmployeeId,
        FullName = $"{e.EFirstname} {e.ELastname}"
      }); // query the list of employees

      ViewData["EmployeeId"] = new SelectList(emp, "EmployeeId", "FullName"); // populate the employee dropdown list
      ViewData["ContactId"] = new SelectList(_context.Contacts, "ContactId", "Cname"); // populate the contact dropdown list
    }

    [Authorize]
    // GET: Orders
    public async Task<IActionResult> Index()
    {
      var orders = await _context.Orders
          .Include(o => o.Contact)
          .Include(o => o.Employee)
          .ToListAsync(); // query all orders including their contact and employee

      return View(orders); // return the view with the list of orders
    }

    [Authorize]
    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int id)
    {
      var order = await _context.Orders
          .Include(o => o.Contact)
          .Include(o => o.Employee)
          .Include(o => o.ProductsOrders)
              .ThenInclude(po => po.Product)
                  .ThenInclude(p => p.Brand)
          .FirstOrDefaultAsync(o => o.OrderId == id); // query the order details including related data

      if (order == null)
      {
        return NotFound(); // return not found if the order does not exist
      }

      return View(order); // return the view with the order details
    }

    [Authorize]
    public IActionResult SalesInvoicePrint(int id)
    {
      var order = _context.Orders
          .Include(o => o.Contact)
          .Include(o => o.Employee)
          .Include(o => o.ProductsOrders)
              .ThenInclude(po => po.Product)
                  .ThenInclude(p => p.Brand)
          .FirstOrDefault(o => o.OrderId == id); // query the order details for printing

      if (order == null)
      {
        return NotFound(); // return not found if the order does not exist
      }

      return View(order); // return the view with the order details for printing
    }

    // GET: Orders/Create
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound(); // return not found if the id is null
      }

      var order = await _context.Orders
          .Include(o => o.ProductsOrders)
          .FirstOrDefaultAsync(o => o.OrderId == id); // query the order including its products orders

      if (order == null)
      {
        return NotFound(); // return not found if the order does not exist
      }

      PopulateViewBags(); // populate view bags for dropdown lists

      var products = _context.Products
          .Include(p => p.Brand)
          .ToList(); // query all products including their brand

      var productSelectList = products.Select(p => new SelectListItem
      {
        Value = p.ProductId.ToString(),
        Text = (p.Brand != null ? p.Brand.Bname + " - " : "") + p.Pname + (p.Size != null ? " (" + p.Size + ")" : "")
      }).ToList(); // create a select list for products

      var productPrices = products.ToDictionary(p => p.ProductId, p => p.Price); // create a dictionary for product prices

      var viewModel = new AddProductsViewModel
      {
        Order = order,
        Products = productSelectList,
        ProductPrices = productPrices,
        ProductsOrders = order.ProductsOrders.ToList()
      }; // initialize the view model with products and order details

      return View(viewModel); // return the view with the view model
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AddProductsViewModel viewModel)
    {
      if (id != viewModel.Order.OrderId)
      {
        return NotFound(); // return not found if the order id does not match
      }

      // manually remove validation for Products and ProductPrices
      ModelState.Remove("Products");
      ModelState.Remove("ProductPrices");

      // manually remove validation for navigation properties in ProductsOrders
      foreach (var productsOrder in viewModel.ProductsOrders)
      {
        ModelState.Remove($"ProductsOrders[{viewModel.ProductsOrders.IndexOf(productsOrder)}].Order");
        ModelState.Remove($"ProductsOrders[{viewModel.ProductsOrders.IndexOf(productsOrder)}].Product");
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(viewModel.Order); // update the order in the context
          await _context.SaveChangesAsync(); // save the changes

          foreach (var productsOrder in viewModel.ProductsOrders)
          {
            var existingProductOrder = await _context.ProductsOrders
                .FirstOrDefaultAsync(po => po.ProductorderId == productsOrder.ProductorderId); // query the existing product order

            if (existingProductOrder != null)
            {
              existingProductOrder.Qty = productsOrder.Qty; // update the quantity of the existing product order
              _context.Update(existingProductOrder); // update the existing product order in the context
            }
            else
            {
              productsOrder.OrderId = viewModel.Order.OrderId; // set the order id for the new products order
              _context.Add(productsOrder); // add the new products order to the context
            }

            var product = await _context.Products.FindAsync(productsOrder.ProductId); // find the product
            product.LocalQty -= productsOrder.Qty; // reduce the local quantity of the product
            _context.Products.Update(product); // update the product
          }

          if (viewModel.ProductsOrdersToRemove != null)
          {
            foreach (var productOrderId in viewModel.ProductsOrdersToRemove)
            {
              var productOrder = await _context.ProductsOrders.FindAsync(productOrderId); // find the product order to remove
              if (productOrder != null)
              {
                _context.ProductsOrders.Remove(productOrder); // remove the product order from the context

                var product = await _context.Products.FindAsync(productOrder.ProductId); // find the product
                if (product != null)
                {
                  product.LocalQty += productOrder.Qty; // increase the local quantity of the product
                  _context.Products.Update(product); // update the product
                }
              }
            }
          }

          await _context.SaveChangesAsync(); // save the changes
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!OrderExists(viewModel.Order.OrderId))
          {
            return NotFound(); // return not found if the order does not exist
          }
          else
          {
            throw; // rethrow the exception if it is not a concurrency exception
          }
        }

        return RedirectToAction(nameof(Index)); // redirect to the index action
      }

      PopulateViewBags(); // populate view bags for dropdown lists

      var products = _context.Products
          .Include(p => p.Brand)
          .ToList(); // query all products including their brand

      viewModel.Products = products.Select(p => new SelectListItem
      {
        Value = p.ProductId.ToString(),
        Text = (p.Brand != null ? p.Brand.Bname + " - " : "") + p.Pname + (p.Size != null ? " (" + p.Size + ")" : "")
      }).ToList(); // create a select list for products

      viewModel.ProductPrices = products.ToDictionary(p => p.ProductId, p => p.Price); // create a dictionary for product prices

      return View(viewModel); // return the view with the view model
    }

    private bool OrderExists(int id)
    {
      return _context.Orders.Any(e => e.OrderId == id); // check if the order exists in the context
    }

    [Authorize]
    // GET: Orders/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound(); // return not found if the id is null
      }

      var order = await _context.Orders
          .Include(o => o.Contact)
          .Include(o => o.Employee)
          .Include(o => o.ProductsOrders)
              .ThenInclude(po => po.Product)
          .FirstOrDefaultAsync(m => m.OrderId == id); // query the order including related data

      if (order == null)
      {
        return NotFound(); // return not found if the order does not exist
      }

      return View(order); // return the view with the order details
    }

    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var order = await _context.Orders
          .Include(o => o.ProductsOrders)
          .FirstOrDefaultAsync(o => o.OrderId == id); // query the order including its products orders

      if (order != null)
      {
        foreach (var productsOrder in order.ProductsOrders)
        {
          var product = await _context.Products.FindAsync(productsOrder.ProductId); // find the product
          product.LocalQty += productsOrder.Qty; // increase the local quantity of the product
          _context.Products.Update(product); // update the product
        }

        _context.Orders.Remove(order); // remove the order from the context
        await _context.SaveChangesAsync(); // save the changes
      }

      return RedirectToAction(nameof(Index)); // redirect to the index action
    }
  }
}
