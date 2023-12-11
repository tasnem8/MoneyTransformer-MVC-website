using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyTransformer.Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Hosting;

namespace MoneyTransformer.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ModelContext _context;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;
            HttpContext.Session.SetString("logo2", homedetails.Image2path);
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");

            ViewBag.RegisUsers = _context.Userlogins.Count(x => x.RoleId == 2);
            ViewBag.banks = _context.Banks.Count();
            ViewBag.wallets = _context.Wallets.Where(x => x.BankId != null).Count();
            ViewBag.transactions = _context.Transactions.Count();

            var walletCounts = from b in _context.Banks
                               join w in _context.Wallets on b.Id equals w.BankId into walletGroup

                               select new { BankName = b.Namee, WalletCount = walletGroup.Count() };

            ViewData["walletscount"] = walletCounts;

            var query = from t in _context.Transactions
                        join w in _context.Wallets on t.SwalletId equals w.Id
                        join ua in _context.Useraccounts on w.UserId equals ua.Id
                        orderby t.TranDate descending
                        select new
                        {
                            UserFName = ua.Fname,
                            UserLName = ua.Lname,
                            WalletNumber = w.Iban,
                            ReceIban = t.RIban,
                            TransactionAmount = t.Amount,
                            TransactionCommission = t.Commission,
                            TransactionDate = t.TranDate
                        };
            ViewData["transactionsinfo"] = query;
            ViewData["transactionsinfocom"] = query;

            // var transactions = _context.Transactions.ToList(); // Replace with your data retrieval logic

            int targetMonth = DateTime.Now.Month;   // Replace with the desired month
            int targetYear = DateTime.Now.Year;
            int targetDay = DateTime.Now.Day;
            ViewBag.m = targetMonth;

            var transactions = _context.Transactions
                .Where(t => t.TranDate.Value.Year == targetYear && t.TranDate.Value.Month == targetMonth && (targetDay - t.TranDate.Value.Day) <= 15)
                .GroupBy(item => new { day = item.TranDate.Value.Day, Month = targetMonth, Year = targetYear })
                .Select(group => new
                {
                    month = new DateTime(group.Key.Year, group.Key.Month, group.Key.day),
                    TotalCommission = group.Sum(t => t.Commission)
                })
                .ToList();
            var jsonData = JsonConvert.SerializeObject(transactions);
            var jsonData2 = JsonConvert.SerializeObject(query);
            ViewBag.JsonData = jsonData;
            ViewBag.JsonData2 = jsonData2;


            return View();
        }

        [HttpPost]
        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;
            HttpContext.Session.SetString("logo2", homedetails.Image2path);
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");

            ViewBag.RegisUsers = _context.Userlogins.Count(x => x.RoleId == 2);
            ViewBag.banks = _context.Banks.Count();
            ViewBag.wallets = _context.Wallets.Count();
            ViewBag.transactions = _context.Transactions.Count();

            var walletCounts = from b in _context.Banks
                               join w in _context.Wallets on b.Id equals w.BankId into walletGroup

                               select new { BankName = b.Namee, WalletCount = walletGroup.Count() };

            ViewData["walletscount"] = walletCounts;

            var query = from t in _context.Transactions
                        join w in _context.Wallets on t.SwalletId equals w.Id
                        join ua in _context.Useraccounts on w.UserId equals ua.Id
                        orderby t.TranDate descending
                        select new
                        {
                            UserFName = ua.Fname,
                            UserLName = ua.Lname,
                            WalletNumber = w.Iban,
                            ReceIban = t.RIban,
                            TransactionAmount = t.Amount,
                            TransactionCommission = t.Commission,
                            TransactionDate = t.TranDate
                        };
            ViewData["transactionsinfo"] = query;
            ViewData["transactionsinfocom"] = query;
            int targetMonth = DateTime.Now.Month;   // Replace with the desired month
            int targetYear = DateTime.Now.Year;
            int targetDay = DateTime.Now.Day;
            ViewBag.m = targetMonth;

            var transactions = _context.Transactions
                .Where(t => t.TranDate.Value.Year == targetYear && t.TranDate.Value.Month == targetMonth && (targetDay - t.TranDate.Value.Day) <= 15)
                .GroupBy(item => new { day = item.TranDate.Value.Day, Month = targetMonth, Year = targetYear })
                .Select(group => new
                {
                    month = new DateTime(group.Key.Year, group.Key.Month, group.Key.day),
                    TotalCommission = group.Sum(t => t.Commission)
                })
                .ToList();
            var jsonData = JsonConvert.SerializeObject(transactions);
            var jsonData2 = JsonConvert.SerializeObject(query);
            ViewBag.JsonData = jsonData;
            ViewBag.JsonData2 = jsonData2;

            if (startDate == null && endDate == null)
            {

                ViewData["transactionsinfo"] = query;
                return View();
            }
            else if (startDate != null && endDate == null)
            {

                var result = query.Where(x => x.TransactionDate.Value.Date >= startDate);
                ViewData["transactionsinfo"] = result;
                return View();
            }
            else if (startDate == null && endDate != null)
            {
                var result = query.Where(x => x.TransactionDate.Value.Date <= endDate);
                ViewData["transactionsinfo"] = result;
                return View();
            }
            else if (startDate != null && endDate != null)
            {
                var result = query.Where(x => x.TransactionDate.Value.Date <= endDate && x.TransactionDate.Value.Date >= startDate);
                ViewData["transactionsinfo"] = result;
                return View();
            }
            return View();
        }
        public async Task<IActionResult> RegisteredUser()
        {
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();

            ViewBag.logo2 = homedetails.Image2path;
            HttpContext.Session.SetString("logo2", homedetails.Image2path);
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");

            var Useraccounts = _context.Useraccounts.ToList();
            var Userlogin = _context.Userlogins.ToList();

            var join = from l in Userlogin
                       join a in Useraccounts on l.UserId equals a.Id
                       join w in _context.Wallets on a.Id equals w.UserId
                       where w.BankId != null && l.RoleId == 2
                       group w by new { a, l } into g

                       select new UserInfoWallet { userlogin = g.Key.l, useraccount = g.Key.a, CountWallet = g.Count() };

            ////
            ///
            var userWalletInfo = from ua in _context.Userlogins.Where(x => x.RoleId == 2)
                                 join ul in _context.Useraccounts on ua.UserId equals ul.Id
                                 join w in _context.Wallets.Where(wallet => wallet.BankId != null)
                                      on ua.UserId equals w.UserId into userWallets
                                 select new UserInfoWallet
                                 {
                                     userlogin = ua,
                                     useraccount = ul,

                                     CountWallet = userWallets.Count()
                                 };

            var usersWithoutWallet = from ua in _context.Userlogins.Where(x => x.RoleId == 2)
                                     join ul in _context.Useraccounts on ua.UserId equals ul.Id
                                     join w in _context.Wallets
                                            on ua.UserId equals w.UserId into userWallets
                                     where !userWallets.Any()
                                     select new UserInfoWallet
                                     {
                                         userlogin = ua,
                                         useraccount = ul,
                                         CountWallet = 0
                                     };
            var usersWithNoBankid = from ua in _context.Userlogins.Where(x => x.RoleId == 2)
                                    join ul in _context.Useraccounts on ua.UserId equals ul.Id
                                    join w in _context.Wallets.Where(wallet => wallet.BankId == null)
                                            on ua.UserId equals w.UserId

                                    select new UserInfoWallet
                                    {
                                        userlogin = ua,
                                        useraccount = ul,
                                        CountWallet = 0
                                    };
            var join1 = userWalletInfo.Concat(usersWithoutWallet).Concat(usersWithNoBankid).Distinct();

            var join2 = join1.GroupBy(row => row.userlogin.Email)
                             .Select(group => group.OrderByDescending(row => row.CountWallet).First());

            ////////
            return join2 != null ?
                        View(join2) :
                        Problem("Entity set 'ModelContext.Useraccounts'  is null.");
        }

        public async Task<IActionResult> ViewAdmins()
        {
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();

            ViewBag.logo2 = homedetails.Image2path;
            HttpContext.Session.SetString("logo2", homedetails.Image2path);
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");

            var Useraccounts = _context.Useraccounts.ToList();
            var Userlogin = _context.Userlogins.ToList();

            var join = from l in Userlogin
                       join a in Useraccounts on l.UserId equals a.Id
                       where l.RoleId == 1
                       select new UserInfo { useraccount = a, userlogin = l /*Fname = a.Fname, Lname = a.Lname, ImagePath = a.ImagePath ,email =l.Email*/};

            return join != null ?
                        View(join) :
                        Problem("Entity set 'ModelContext.Useraccounts'  is null.");
        }
        public IActionResult AddAdmin()
        {
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();

            ViewBag.logo2 = homedetails.Image2path;
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin([Bind("Id,Fname,Lname,ImagePath,ImageFile")] Useraccount useraccount, string Email, string password, string compassword)
        {
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();

            ViewBag.logo2 = homedetails.Image2path;
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            if (password != compassword)
            {
                ViewData["check"] = "Passwords not match";
                return View(useraccount);
            }
            var accounts = _context.Userlogins.Where(x => x.Email == Email).FirstOrDefault();
            if (accounts != null)
            {
                ViewData["Messag2"] = "Email is used to another account";
                return View(useraccount);

            }
            if (ModelState.IsValid)
            {
                if (useraccount.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + useraccount.ImageFile.FileName;

                    string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await useraccount.ImageFile.CopyToAsync(fileStream);
                    }
                    useraccount.ImagePath = fileName;


                }
                else
                {
                    // Replace "image.jpg" with the actual image file's name
                    string imageName = "profilePic.png";
                    // Generate a unique filename using Guid

                    useraccount.ImagePath = imageName;



                }
                _context.Add(useraccount);
                await _context.SaveChangesAsync();
                Userlogin userLogin = new Userlogin();
                userLogin.Email = Email;
                userLogin.Passwordd = password;
                userLogin.UserId = useraccount.Id;
                userLogin.RoleId = 1;

                _context.Add(userLogin);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(ViewAdmins));

            }
            return View(useraccount);
        }




        public async Task<IActionResult> ViewMessages()
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            return _context.Contactus != null ?
                        View(await _context.Contactus.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Contactus'  is null.");
        }

        public async Task<IActionResult> CustomerTestimonials()
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");

            var modelContext = _context.Testimonials.OrderByDescending(x => x.TestimonialDate).Include(t => t.User);
            return View(await modelContext.ToListAsync());
        }

        // [ActionName("CustomerTestimonials")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("Admin/CustomerTest")]

        public async Task<IActionResult> CustomerTestimonials( string? option, decimal? id, int? f)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            
            if (ModelState.IsValid)
            {
                var testimonial = _context.Testimonials.Where(x => x.Id == id).OrderByDescending(x => x.TestimonialDate).FirstOrDefault();

                try
                {
                    if (option != null)
                    {
                        if (option == "option1")
                        {
                            var modelContext = _context.Testimonials.Where(t => t.Status == "Posted").OrderByDescending(x => x.TestimonialDate).Include(t => t.User).ToList();
                            return View(modelContext);
                        }
                        else if (option == "option2")
                        {
                            var modelContext = _context.Testimonials.Where(t => t.Status == "onhold").OrderByDescending(x=>x.TestimonialDate).Include(t => t.User).ToList();
                            return View(modelContext);
                        }
                     
                    }
                    else
                    {

                        if (f == 1)
                        {
                            testimonial.Status = "Posted";
                            _context.Update(testimonial);
                            await _context.SaveChangesAsync();
                        }
                        else if (f == 2)
                        {
                            if (testimonial != null)
                            {
                                _context.Testimonials.Remove(testimonial);
                            }

                            await _context.SaveChangesAsync();
                        }
                    }

                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CustomerTestimonials));
            }
            return View();
        }
    
        [HttpPost]
[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTestimonial(decimal id)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");

            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CustomerTestimonials));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostTestimonial(decimal id, int f)
        {

            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");


            if (ModelState.IsValid)
            {
                var testimonial = _context.Testimonials.Where(x => x.Id == id).FirstOrDefault();
                try
                {
                    
                    testimonial.Status = "Posted";
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CustomerTestimonials));
            }
            return View();
        }


       
        // GET: Homes/Edit/5
        public async Task<IActionResult> HomeDetails(decimal? id)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            if (id == null || _context.Homes == null)
            {
                return NotFound();
            }

            var home = await _context.Homes.FindAsync(id);
            
            if (home == null)
            {
                return NotFound();
            }
            return View(home);
        }

        // POST: Homes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HomeDetails(decimal id, [Bind("Id,Text1,Text2,ImageFile1,LogoFile,ImageFile2")] Home home)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            if (id != home.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    var h = _context.Homes.Where(x => x.Id == id).FirstOrDefault();
                    
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (home.ImageFile1 != null)
                    {
                        var im = await _context.Homes.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
                        //if (im != null)
                        //{
                        //    string path1 = Path.Combine(wwwRootPath + "/Images/" + im.Image1path);
                        //    Console.Write(path1);
                        //    System.IO.File.Delete(path1);
                        //}
                        string fileName = Guid.NewGuid().ToString() + home.ImageFile1.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile1.CopyToAsync(fileStream);
                        }

                        h.Image1path = fileName;
                    
                    }

                    if (home.LogoFile != null)
                    {
                        var im = await _context.Homes.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
                        //if (im != null)
                        //{
                        //    string path1 = Path.Combine(wwwRootPath + "/Images/" + im.Logopath);
                        //    Console.Write(path1);
                        //    System.IO.File.Delete(path1);
                        //}
                        string fileName = Guid.NewGuid().ToString() + home.LogoFile.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.LogoFile.CopyToAsync(fileStream);
                        }

                        h.Logopath = fileName;
                

                    }
                    if (home.ImageFile2 != null)
                    {
                        var im = await _context.Homes.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
                        //if (im != null)
                        //{
                        //    string path1 = Path.Combine(wwwRootPath + "/Images/" + im.Image1path);
                        //    Console.Write(path1);
                        //    System.IO.File.Delete(path1);
                        //}
                        string fileName = Guid.NewGuid().ToString() + home.ImageFile2.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile2.CopyToAsync(fileStream);
                        }

                        h.Image2path = fileName;

                    }
                    h.Text1 = home.Text1;
                    home = h;
                    _context.Update(home);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeExists(home.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(HomeDetails));
            }
            return View(home);
        }
        public async Task<IActionResult> ContactDetails()
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            decimal id = 1;
            if (id == null || _context.Homes == null)
            {
                return NotFound();
            }

            var home = await _context.Homes.FindAsync(id);
          
            if (home == null)
            {
                return NotFound();
            }
            return View(home);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactDetails(decimal? id, [Bind("Id,Email,Phonenumber,Facebooklink,Instagramlink")] Home home)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            if (id != home.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var h = _context.Homes.Where(x => x.Id == id).FirstOrDefault();

                    h.Phonenumber = home.Phonenumber;
                    h.Facebooklink = home.Facebooklink;
                    h.Email = home.Email;
                    h.Instagramlink = home.Instagramlink;
                    home = h;
                    _context.Update(home);
                 

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeExists(home.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ContactDetails));
            }
            return View(home);
        }

        // GET: Aboutus/Edit/5
        public async Task<IActionResult> AboutusDetails()
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            decimal id = 1;
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus.FindAsync(id);
            if (aboutu == null)
            {
                return NotFound();
            }
            return View(aboutu);
        }

        // POST: Aboutus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AboutusDetails(decimal id, [Bind("Id,Text1,Text2,Text3,Text4,ImageFile1,ImageFile2,ImageFile3")] Aboutu aboutu)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            if (id != aboutu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var h = _context.Aboutus.Where(x => x.Id == id).FirstOrDefault();

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (aboutu.ImageFile1 != null)
                    {
                        var im = await _context.Homes.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
                        //if (im != null)
                        //{
                        //    string path1 = Path.Combine(wwwRootPath + "/Images/" + im.Image1path);
                        //    Console.Write(path1);
                        //    System.IO.File.Delete(path1);
                        //}
                        string fileName = Guid.NewGuid().ToString() + aboutu.ImageFile1.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await aboutu.ImageFile1.CopyToAsync(fileStream);
                        }

                        h.Image1path = fileName;

                    }
                    if (aboutu.ImageFile2 != null)
                    {
                        var im = await _context.Homes.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
                        //if (im != null)
                        //{
                        //    string path1 = Path.Combine(wwwRootPath + "/Images/" + im.Image1path);
                        //    Console.Write(path1);
                        //    System.IO.File.Delete(path1);
                        //}
                        string fileName = Guid.NewGuid().ToString() + aboutu.ImageFile2.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await aboutu.ImageFile2.CopyToAsync(fileStream);
                        }

                        h.Image2path = fileName;

                    }
                    if (aboutu.ImageFile3 != null)
                    {
                        var im = await _context.Homes.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
                        //if (im != null)
                        //{
                        //    string path1 = Path.Combine(wwwRootPath + "/Images/" + im.Image1path);
                        //    Console.Write(path1);
                        //    System.IO.File.Delete(path1);
                        //}
                        string fileName = Guid.NewGuid().ToString() + aboutu.ImageFile3.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await aboutu.ImageFile3.CopyToAsync(fileStream);
                        }

                        h.Image3path = fileName;

                    }
                    h.Text1 = aboutu.Text1;
                    h.Text2 = aboutu.Text2;
                    h.Text3 = aboutu.Text3;
                    aboutu = h;
                    _context.Update(aboutu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutuExists(aboutu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AboutusDetails));
            }
            return View(aboutu);
        }

        public async Task<IActionResult> Report()
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");

            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            ///MONTH REPORTE DATA
            ///number of tran/month
            var currentDate = DateTime.Today;
            ViewBag.DateNow = currentDate.Date;
            ViewBag.month = currentDate.Month;
            ViewBag.year = currentDate.Year;
            var transactionMonCount = _context.Transactions
                .Where(t =>
                    t.TranDate.Value.Year == currentDate.Year &&
                    t.TranDate.Value.Month == currentDate.Month)
                .Count();

            ViewBag.transactionMonCount = transactionMonCount;
            //per tran 
            var transactionPreMonCount = _context.Transactions
                .Where(t =>
                    t.TranDate.Value.Year == currentDate.Year &&
                    t.TranDate.Value.Month == currentDate.Month - 1)
                .Count();
            
                double percentagPreNow = 0;
            if (transactionPreMonCount != 0)
            {  percentagPreNow = (((int)transactionMonCount - (int)transactionPreMonCount) / (int)transactionPreMonCount )* 100; }
            ViewBag.percentagPreNow = percentagPreNow;

            //number of registred
            ViewBag.RegisUsers = _context.Userlogins.Count(x => x.RoleId == 2);
            //number of wallet created
            ViewBag.Wal = _context.Wallets.Where(t =>
                    t.Datecreate.Value.Year == currentDate.Year &&
                    t.Datecreate.Value.Month == currentDate.Month).Count();

            //number actived wallets
            ViewBag.ActivWal = _context.Wallets.Where(t => t.Status == "Active" && t.BankId!=null && 
                    t.Datecreate.Value.Year == currentDate.Year &&
                    t.Datecreate.Value.Month == currentDate.Month).Count();

            //number of messages
            ViewBag.messages = _context.Contactus.Count();
            ///reviews sent
            ViewBag.test = _context.Testimonials.Where((t =>
                    t.TestimonialDate.Value.Year == currentDate.Year &&
                    t.TestimonialDate.Value.Month == currentDate.Month)).Count();

            //reviews Posted
            ViewBag.posttesti = _context.Testimonials.Where(t=>t.Status=="Posted" &&
                    t.TestimonialDate.Value.Year == currentDate.Year &&
                    t.TestimonialDate.Value.Month == currentDate.Month).Count();

            //total revenues
            ViewBag.thisMonthReven = _context.Transactions.Where(t =>
                    t.TranDate.Value.Year == currentDate.Year &&
                    t.TranDate.Value.Month == currentDate.Month).Sum(x => x.Commission);
            //per reven
            decimal? preMonthReven = _context.Transactions.Where(t =>
                    t.TranDate.Value.Year == currentDate.Year &&
                    t.TranDate.Value.Month == currentDate.Month-1).Sum(x => x.Commission);
            var thisMonthReven = ViewBag.thisMonthReven;
            double PreThisPerRev =0;
            if (preMonthReven!=0)
              PreThisPerRev =(((double)thisMonthReven - (double)preMonthReven)/ (double)preMonthReven)*100;
            ViewBag.PreThisPerRev = Math.Round( PreThisPerRev,2);


            ///YEAR REPORTE DATA
            ///number of tran/month
            ///
            var transactionYearCount = _context.Transactions
                .Where(t =>
                    t.TranDate.Value.Year == currentDate.Year )
                .Count();

            ViewBag.transactionYearCount = transactionYearCount;
            //per tran 
            var transactionPreYearCount = _context.Transactions
                .Where(t =>
                    t.TranDate.Value.Year == currentDate.Year - 1)
                .Count();

            double Y_percentagPreNow = 0;
            if (transactionPreYearCount != 0)
            { Y_percentagPreNow = (((int)transactionYearCount - (int)transactionPreYearCount) / (int)transactionPreYearCount) * 100; }
            ViewBag.y_percentagPreNow = Y_percentagPreNow;

            //number of all registred users
            ViewBag.RegisUsers = _context.Userlogins.Count(x => x.RoleId == 2);
            //number of wallet created/year
            ViewBag.Y_Wal = _context.Wallets.Where(t =>
                    t.Datecreate.Value.Year == currentDate.Year ).Count();

            //number actived wallets/Year
            ViewBag.Y_ActivWal = _context.Wallets.Where(t => t.Status == "Active" &&
                    t.Datecreate.Value.Year == currentDate.Year).Count();

            //number of all messages
            ViewBag.messages = _context.Contactus.Count();
            ///reviews sent
            ViewBag.y_test = _context.Testimonials.Where(t =>
                    t.TestimonialDate.Value.Year == currentDate.Year ).Count();

            //reviews Posted
            ViewBag.y_posttesti = _context.Testimonials.Where(t => t.Status == "Posted" &&
                    t.TestimonialDate.Value.Year == currentDate.Year ).Count();

            //total revenues
            ViewBag.y_thisYearReven = _context.Transactions.Where(t =>
                    t.TranDate.Value.Year == currentDate.Year).Sum(x => x.Commission);

            //per reven
            decimal? y_preYearReven = _context.Transactions.Where(t =>
                    t.TranDate.Value.Year == currentDate.Year -1).Sum(x => x.Commission);

            var y_thisYearReven = ViewBag.y_thisYearReven;
            double y_PreThisPerRev = 0;
            if (y_preYearReven != 0)
                y_PreThisPerRev =Math.Round( (((double)y_thisYearReven - (double)y_preYearReven) / (double)y_preYearReven) * 100,2);
            ViewBag.y_PreThisPerRev = y_PreThisPerRev;


            //////

       

            // var transactions = _context.Transactions.ToList(); //
            var transactionsDay = _context.Transactions
                            .GroupBy(t => t.TranDate.Value.Date)
                            .Select(group => new
                            {
                                Date = group.Key,
                                CommissionCount = group.Sum(t => t.Commission)
                            })
                            .OrderBy(entry => entry.Date)
                            .ToList();
            var jsonData3 = JsonConvert.SerializeObject(transactionsDay);
            ViewBag.JsonData3 = jsonData3;

            int targetMonth = DateTime.Now.Month;   
            int targetYear = DateTime.Now.Year;  
            ViewBag.thisMonth = targetMonth;
            ViewBag.thisYear = targetYear;

            var commissionBymo = _context.Transactions
                .Where(t =>  t.TranDate.Value.Year == targetYear)
                .GroupBy(item => new { Month = item.TranDate.Value.Month, Year = targetYear })
                .Select(group => new
                {
                    month = new DateTime(group.Key.Year, group.Key.Month,1),
                    TotalCommission = group.Sum(t => t.Commission)
                })
                .ToList();

            var jsonData4 = JsonConvert.SerializeObject(commissionBymo);
            ViewBag.JsonData4 = jsonData4;

            var commissionByYear = _context.Transactions
               .GroupBy(t => t.TranDate.Value.Year)
               .Select(group => new
               {
                   years = group.Key,
                   TotalCommission = group.Sum(t => t.Commission)
               })
               .ToList();

            //.Where(t => t.TranDate.Value.Year == targetYear)

            var jsonData5 = JsonConvert.SerializeObject(commissionByYear);
            ViewBag.JsonData5 = jsonData5;
            return View();
        }
            private bool HomeExists(decimal id)
        {
            return (_context.Homes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool TestimonialExists(decimal id)
        {
            return (_context.Testimonials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool AboutuExists(decimal id)
        {
            return (_context.Aboutus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
