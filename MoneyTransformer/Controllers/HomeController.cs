using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTransformer.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace MoneyTransformer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;

        }
        
      
        public async Task<IActionResult> Index()
        {
            var id = 1;
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;

            ViewBag.logo = homedetails.Logopath;
            ViewBag.instagram = homedetails.Instagramlink;
            ViewBag.facebook = homedetails.Facebooklink;
            ViewBag.companyemail = homedetails.Email;
            ViewBag.phonenumber = homedetails.Phonenumber;
            HttpContext.Session.SetString("logo", homedetails.Logopath);
            HttpContext.Session.SetString("logo2", homedetails.Image2path);
            HttpContext.Session.SetString("instagram", homedetails.Instagramlink);
            HttpContext.Session.SetString("facebook", homedetails.Facebooklink);
            HttpContext.Session.SetString("companyemail", homedetails.Email);
            HttpContext.Session.SetString("phonenumber", homedetails.Phonenumber);

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            ViewBag.image = HttpContext.Session.GetString("Image");

            var testimonial = _context.Testimonials.Include(t => t.User);
            var home =  _context.Homes.ToList();
            var banks = _context.Banks.ToList();
            var modelContext = Tuple.Create<IEnumerable<Testimonial>, IEnumerable<Home>, IEnumerable<Bank>>(testimonial,home,banks);
            return View(modelContext);
        }

        // GET: Contactus/Create
        public IActionResult ContactUs()
        {
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;

            ViewBag.logo = homedetails.Logopath;
            ViewBag.instagram = homedetails.Instagramlink;
            ViewBag.facebook = homedetails.Facebooklink;
            ViewBag.companyemail = homedetails.Email;
            ViewBag.phonenumber = homedetails.Phonenumber;
            HttpContext.Session.SetString("logo", homedetails.Logopath);
            HttpContext.Session.SetString("logo2", homedetails.Image2path);
            HttpContext.Session.SetString("instagram", homedetails.Instagramlink);
            HttpContext.Session.SetString("facebook", homedetails.Facebooklink);
            HttpContext.Session.SetString("companyemail", homedetails.Email);
            HttpContext.Session.SetString("phonenumber", homedetails.Phonenumber);

            ViewBag.instagram = HttpContext.Session.GetString("instagram");
            ViewBag.facebook = HttpContext.Session.GetString("facebook");
            ViewBag.companyemail = HttpContext.Session.GetString("companyemail");
            ViewBag.phonenumber = HttpContext.Session.GetString("phonenumber");
            ///user session
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            return View();
        }

        // POST: Contactus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs([Bind("Id,Message,Subject,Email,Phonenumber,Name")] Contactu contactu)
        {
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;

            ViewBag.logo = homedetails.Logopath;
            ViewBag.instagram = homedetails.Instagramlink;
            ViewBag.facebook = homedetails.Facebooklink;
            ViewBag.companyemail = homedetails.Email;
            ViewBag.phonenumber = homedetails.Phonenumber;
            HttpContext.Session.SetString("logo", homedetails.Logopath);
            HttpContext.Session.SetString("logo2", homedetails.Image2path);
            HttpContext.Session.SetString("instagram", homedetails.Instagramlink);
            HttpContext.Session.SetString("facebook", homedetails.Facebooklink);
            HttpContext.Session.SetString("companyemail", homedetails.Email);
            HttpContext.Session.SetString("phonenumber", homedetails.Phonenumber);

            ///contact session
            ViewBag.logo = HttpContext.Session.GetString("logo");
            ViewBag.instagram = HttpContext.Session.GetString("instagram");
            ViewBag.facebook = HttpContext.Session.GetString("facebook");
            ViewBag.companyemail = HttpContext.Session.GetString("companyemail");
            ViewBag.phonenumber = HttpContext.Session.GetString("phonenumber");
            ///user session
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            if (ModelState.IsValid)
            {
                _context.Add(contactu);
                await _context.SaveChangesAsync();
                ViewData["ContactDone"] = "Your Message is Sent succsefully";
                return RedirectToAction(nameof(ContactUs));
            }
            return View(contactu);
        }


        public async Task<IActionResult> About()
        {
            decimal id = 1;
            ///contact session
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;

            ViewBag.logo = homedetails.Logopath;
            ViewBag.instagram = homedetails.Instagramlink;
            ViewBag.facebook = homedetails.Facebooklink;
            ViewBag.companyemail = homedetails.Email;
            ViewBag.phonenumber = homedetails.Phonenumber;
            HttpContext.Session.SetString("logo", homedetails.Logopath);
            HttpContext.Session.SetString("logo2", homedetails.Image2path);
            HttpContext.Session.SetString("instagram", homedetails.Instagramlink);
            HttpContext.Session.SetString("facebook", homedetails.Facebooklink);
            HttpContext.Session.SetString("companyemail", homedetails.Email);
            HttpContext.Session.SetString("phonenumber", homedetails.Phonenumber);

            ViewBag.instagram = HttpContext.Session.GetString("instagram");
            ViewBag.facebook = HttpContext.Session.GetString("facebook");
            ViewBag.companyemail = HttpContext.Session.GetString("companyemail");
            ViewBag.phonenumber = HttpContext.Session.GetString("phonenumber");
            ///user session
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");

            var aboutu = await _context.Aboutus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutu == null)
            {
                return NotFound();
            }

            return View(aboutu);
            //return View();
        }
        public IActionResult Privacy()
        {///contact session
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;
            ViewBag.logo = HttpContext.Session.GetString("logo");
            ViewBag.instagram = HttpContext.Session.GetString("instagram");
            ViewBag.facebook = HttpContext.Session.GetString("facebook");
            ViewBag.companyemail = HttpContext.Session.GetString("companyemail");
            ViewBag.phonenumber = HttpContext.Session.GetString("phonenumber");
            ///user session
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}