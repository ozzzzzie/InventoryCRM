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
  public class ContactsController : Controller
  {
    private readonly NaimsdbContext _context;

    public ContactsController(NaimsdbContext context)
    {
      _context = context;
    }

    //authorisation rule added to ensure that a user is logged in
    [Authorize]
    // GET: Contacts
    public async Task<IActionResult> Index()
    {
      return View(await _context.Contacts.ToListAsync());
    }

    [Authorize]
    public async Task<IActionResult> Salons()
    {
      //getting all salons
      var salons = await _context.Contacts.Where(c => c.Ctype == "Salon").ToListAsync();
      return View(salons);
    }

    [Authorize]
    public async Task<IActionResult> Influencers()
    {
      //getting all influencers
      var influencers = await _context.Contacts.Where(c => c.Ctype == "Influencer").ToListAsync();
      return View(influencers);
    }

    [Authorize]
    // GET: Contacts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var contact = await _context.Contacts
          .FirstOrDefaultAsync(m => m.ContactId == id);
      if (contact == null)
      {
        return NotFound();
      }

      return View(contact);
    }

    //authorisation rule added to ensure that a user is logged into SuperAdmin or Manager accounts before they can create an influencer/salon

    [Authorize(Roles = "SuperAdmin,Manager")]
    // GET: Contacts/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Contacts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ContactId,Cname,Ctype,Caddress,Email,PhoneNumber")] Contact contact)
    {
      if (ModelState.IsValid)
      {
        _context.Add(contact);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(contact);
    }
    [Authorize(Roles = "SuperAdmin,Manager")]
    // GET: Contacts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var contact = await _context.Contacts.FindAsync(id);
      if (contact == null)
      {
        return NotFound();
      }
      return View(contact);
    }

    // POST: Contacts/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ContactId,Cname,Ctype,Caddress,Email,PhoneNumber")] Contact contact)
    {
      if (id != contact.ContactId)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(contact);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ContactExists(contact.ContactId))
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
      return View(contact);
    }
     [Authorize(Roles = "SuperAdmin,Manager")]
    // GET: Contacts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var contact = await _context.Contacts
          .FirstOrDefaultAsync(m => m.ContactId == id);
      if (contact == null)
      {
        return NotFound();
      }

      return View(contact);
    }

    // POST: Contacts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var contact = await _context.Contacts.FindAsync(id);
      if (contact != null)
      {
        _context.Contacts.Remove(contact);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool ContactExists(int id)
    {
      return _context.Contacts.Any(e => e.ContactId == id);
    }
  }
}
