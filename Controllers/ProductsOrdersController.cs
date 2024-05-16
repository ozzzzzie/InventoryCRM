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
    public class ProductsOrdersController : Controller
    {
        private readonly NaimsdbContext _context;

        public ProductsOrdersController(NaimsdbContext context)
        {
            _context = context;
        }

        // GET: ProductsOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductsOrders.ToListAsync());
        }

        // GET: ProductsOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsOrder = await _context.ProductsOrders
                .FirstOrDefaultAsync(m => m.ProductorderId == id);
            if (productsOrder == null)
            {
                return NotFound();
            }

            return View(productsOrder);
        }

        // GET: ProductsOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductsOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductorderId,OrderId,ProductId,Qty")] ProductsOrder productsOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productsOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productsOrder);
        }

        // GET: ProductsOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsOrder = await _context.ProductsOrders.FindAsync(id);
            if (productsOrder == null)
            {
                return NotFound();
            }
            return View(productsOrder);
        }

        // POST: ProductsOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductorderId,OrderId,ProductId,Qty")] ProductsOrder productsOrder)
        {
            if (id != productsOrder.ProductorderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsOrderExists(productsOrder.ProductorderId))
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
            return View(productsOrder);
        }

        // GET: ProductsOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsOrder = await _context.ProductsOrders
                .FirstOrDefaultAsync(m => m.ProductorderId == id);
            if (productsOrder == null)
            {
                return NotFound();
            }

            return View(productsOrder);
        }

        // POST: ProductsOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productsOrder = await _context.ProductsOrders.FindAsync(id);
            if (productsOrder != null)
            {
                _context.ProductsOrders.Remove(productsOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsOrderExists(int id)
        {
            return _context.ProductsOrders.Any(e => e.ProductorderId == id);
        }
    }
}
