using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Repository;
using Microsoft.AspNetCore.Authorization;
using Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/")]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        //private readonly WebAppContext _context;
        private ContactService _service;

        public ContactsController(ContactService service)
        {
            _service = service;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
              return _context.Contact != null ? 
                          View(await _context.Contact.ToListAsync()) :
                          Problem("Entity set 'WebAppContext.Contact'  is null.");
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return Problem();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.ContactUsername == id);
            if (contact == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return View(contact);
        }

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
        public async Task<IActionResult> Create([Bind("ContactUsername")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ContactUsername")] Contact contact)
        {
            if (id != contact.ContactUsername)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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
                    if (!ContactExists(contact.ContactUsername))
                    {
                        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.ContactUsername == id);
            if (contact == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Contact == null)
            {
                return Problem("Entity set 'WebAppContext.Contact'  is null.");
            }
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(string id)
        {
          return (_context.Contact?.Any(e => e.ContactUsername == id)).GetValueOrDefault();
        }
    }
}
