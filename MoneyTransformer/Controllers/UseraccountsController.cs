using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyTransformer.Models;

namespace MoneyTransformer.Controllers
{
    public class UseraccountsController : Controller
    {
        private readonly ModelContext _context;

        public UseraccountsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Useraccounts
        public async Task<IActionResult> Index()
        {
              return _context.Useraccounts != null ? 
                          View(await _context.Useraccounts.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Useraccounts'  is null.");
        }

        // GET: Useraccounts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Useraccounts == null)
            {
                return NotFound();
            }

            var useraccount = await _context.Useraccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useraccount == null)
            {
                return NotFound();
            }

            return View(useraccount);
        }

        // GET: Useraccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Useraccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fname,Lname,ImagePath")] Useraccount useraccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(useraccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(useraccount);
        }

        // GET: Useraccounts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Useraccounts == null)
            {
                return NotFound();
            }

            var useraccount = await _context.Useraccounts.FindAsync(id);
            if (useraccount == null)
            {
                return NotFound();
            }
            return View(useraccount);
        }

        // POST: Useraccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Fname,Lname,ImagePath")] Useraccount useraccount)
        {
            if (id != useraccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(useraccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UseraccountExists(useraccount.Id))
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
            return View(useraccount);
        }

        // GET: Useraccounts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Useraccounts == null)
            {
                return NotFound();
            }

            var useraccount = await _context.Useraccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useraccount == null)
            {
                return NotFound();
            }

            return View(useraccount);
        }

        // POST: Useraccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Useraccounts == null)
            {
                return Problem("Entity set 'ModelContext.Useraccounts'  is null.");
            }
            var useraccount = await _context.Useraccounts.FindAsync(id);
            if (useraccount != null)
            {
                _context.Useraccounts.Remove(useraccount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UseraccountExists(decimal id)
        {
          return (_context.Useraccounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
