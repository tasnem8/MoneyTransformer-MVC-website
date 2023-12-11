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
    public class VirtualBanksController : Controller
    {
        private readonly ModelContext _context;

        public VirtualBanksController(ModelContext context)
        {
            _context = context;
        }

        // GET: VirtualBanks
        public async Task<IActionResult> Index()
        {
              return _context.VirtualBanks != null ? 
                          View(await _context.VirtualBanks.ToListAsync()) :
                          Problem("Entity set 'ModelContext.VirtualBanks'  is null.");
        }

        // GET: VirtualBanks/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.VirtualBanks == null)
            {
                return NotFound();
            }

            var virtualBank = await _context.VirtualBanks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (virtualBank == null)
            {
                return NotFound();
            }

            return View(virtualBank);
        }

        // GET: VirtualBanks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VirtualBanks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cardnumber,Expireddate,Cvv,Balance,Owneremanil,Ownername")] VirtualBank virtualBank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(virtualBank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(virtualBank);
        }

        // GET: VirtualBanks/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.VirtualBanks == null)
            {
                return NotFound();
            }

            var virtualBank = await _context.VirtualBanks.FindAsync(id);
            if (virtualBank == null)
            {
                return NotFound();
            }
            return View(virtualBank);
        }

        // POST: VirtualBanks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Cardnumber,Expireddate,Cvv,Balance,Owneremanil,Ownername")] VirtualBank virtualBank)
        {
            if (id != virtualBank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(virtualBank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VirtualBankExists(virtualBank.Id))
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
            return View(virtualBank);
        }

        // GET: VirtualBanks/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.VirtualBanks == null)
            {
                return NotFound();
            }

            var virtualBank = await _context.VirtualBanks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (virtualBank == null)
            {
                return NotFound();
            }

            return View(virtualBank);
        }

        // POST: VirtualBanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.VirtualBanks == null)
            {
                return Problem("Entity set 'ModelContext.VirtualBanks'  is null.");
            }
            var virtualBank = await _context.VirtualBanks.FindAsync(id);
            if (virtualBank != null)
            {
                _context.VirtualBanks.Remove(virtualBank);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VirtualBankExists(decimal id)
        {
          return (_context.VirtualBanks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
