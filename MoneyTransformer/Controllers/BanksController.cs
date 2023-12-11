using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyTransformer.Models;

namespace MoneyTransformer.Controllers
{
    public class BanksController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ModelContext _context;

        public BanksController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Banks
        public async Task<IActionResult> Index()
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            return _context.Banks != null ?
                          View(await _context.Banks.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Banks'  is null.");
        }

        // GET: Banks/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            if (id == null || _context.Banks == null)
            {
                return NotFound();
            }

            var bank = await _context.Banks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bank == null)
            {
                return NotFound();
            }

            return View(bank);
        }

        // GET: Banks/Create
        public IActionResult Create()
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            return View();
        }

        // POST: Banks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Namee,Descreption,Phonenumber,ImageFile1")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (bank.ImageFile1 != null)
                {

                    string fileName = Guid.NewGuid().ToString() + bank.ImageFile1.FileName;

                    string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await bank.ImageFile1.CopyToAsync(fileStream);
                    }

                    bank.Imagepath = fileName;

                }
                _context.Add(bank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bank);
        }

        // GET: Banks/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");

            if (id == null || _context.Banks == null)
            {
                return NotFound();
            }

            var bank = await _context.Banks.FindAsync(id);
            if (bank == null)
            {
                return NotFound();
            }
            return View(bank);
        }

        // POST: Banks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Namee,Descreption,Phonenumber,ImageFile1,Imagepath")] Bank bank)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            //var h = _context.Banks.Where(x => x.Id == id).FirstOrDefault();

            if (id != bank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (bank.ImageFile1 != null)
                    {
                        var im = await _context.Homes.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
                        //if (im != null)
                        //{
                        //    string path1 = Path.Combine(wwwRootPath + "/Images/" + im.Image1path);
                        //    Console.Write(path1);
                        //    System.IO.File.Delete(path1);
                        //}
                        string fileName = Guid.NewGuid().ToString() + bank.ImageFile1.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await bank.ImageFile1.CopyToAsync(fileStream);
                        }

                        bank.Imagepath = fileName;

                    }
                    //else
                    //{

                    //    bank.Imagepath = h.Imagepath;
                    //}
                   // bank = h;
                    _context.Update(bank);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankExists(bank.Id))
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
            return View(bank);
        }

        // GET: Banks/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            if (id == null || _context.Banks == null)
            {
                return NotFound();
            }

            var bank = await _context.Banks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bank == null)
            {
                return NotFound();
            }

            return View(bank);
        }

        // POST: Banks/Delete/5
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Banks == null)
            {
                return Problem("Entity set 'ModelContext.Banks'  is null.");
            }
            var bank = await _context.Banks.FindAsync(id);
            if (bank != null)
            {
                _context.Banks.Remove(bank);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankExists(decimal id)
        {
            return (_context.Banks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
