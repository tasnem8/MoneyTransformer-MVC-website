using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTransformer.Models;

namespace MoneyTransformer.Controllers
{
    public class UpdateInfoController : Controller
    {


        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public UpdateInfoController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }




        public async Task<IActionResult> DisplayInfo()
        {
            ///contact session
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo = homedetails.Logopath;
            HttpContext.Session.SetString("logo", homedetails.Logopath);
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");
            ViewBag.instagram = HttpContext.Session.GetString("instagram");
            ViewBag.facebook = HttpContext.Session.GetString("facebook");
            ViewBag.companyemail = HttpContext.Session.GetString("companyemail");
            ViewBag.phonenumber = HttpContext.Session.GetString("phonenumber");
            ///user session
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");


            var r = HttpContext.Session.GetInt32("roleId");
            var rolee = _context.Roles.Where(x => x.Id == r).FirstOrDefault();
            ViewBag.rolename = rolee.Rolename;
            ViewBag.email = HttpContext.Session.GetString("Email");
            var id = HttpContext.Session.GetInt32("UserId");
            if (id == null || _context.Useraccounts == null || HttpContext.Session.GetString("Email") == null)
            {
                return NotFound();
            }

            var customer = await _context.Useraccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            var query = from t in _context.Transactions
                        join w in _context.Wallets on t.SwalletId equals w.Id
                       
                        join ua in _context.Useraccounts on w.UserId equals ua.Id
                        orderby t.TranDate descending
                        where w.UserId == id
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
            ViewData["transCustInfo"] = query;
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> DisplayInfo(DateTime? startDate, DateTime? endDate)
        {
            
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo = homedetails.Logopath;
            HttpContext.Session.SetString("logo", homedetails.Logopath);
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");
            ViewBag.instagram = HttpContext.Session.GetString("instagram");
            ViewBag.facebook = HttpContext.Session.GetString("facebook");
            ViewBag.companyemail = HttpContext.Session.GetString("companyemail");
            ViewBag.phonenumber = HttpContext.Session.GetString("phonenumber");
            ///user session
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");

            var r = HttpContext.Session.GetInt32("roleId");
            var rolee = _context.Roles.Where(x => x.Id == r).FirstOrDefault();
            ViewBag.rolename = rolee.Rolename;
            ViewBag.email = HttpContext.Session.GetString("Email");
            var id = HttpContext.Session.GetInt32("UserId");
            var customer = await _context.Useraccounts
               .FirstOrDefaultAsync(m => m.Id == id);
            var query = from t in _context.Transactions
                        join w in _context.Wallets on t.SwalletId equals w.Id
                        join ua in _context.Useraccounts on w.UserId equals ua.Id
                        orderby t.TranDate descending
                        where w.UserId == id
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
            ViewData["transCustInfo"] = query;
            if (startDate == null && endDate == null)
            {

                ViewData["transCustInfo"] = query;
            }
            else if (startDate != null && endDate == null)
            {

                var result = query.Where(x => x.TransactionDate.Value.Date >= startDate);
                ViewData["transCustInfo"] = result;
            }
            else if (startDate == null && endDate != null)
            {
                var result = query.Where(x => x.TransactionDate.Value.Date <= endDate);
                ViewData["transCustInfo"] = result;
            }
            else if (startDate != null && endDate != null)
            {
                var result = query.Where(x => x.TransactionDate.Value.Date <= endDate && x.TransactionDate.Value.Date >= startDate);
                ViewData["transCustInfo"] = result;
              
            }
            return View(customer) ;
            //return Redirect("/UpdateInfo/DisplayInfo#recentTra");

        } 
        public async Task<IActionResult> UpdateInfo(decimal? id)
        {

            ///contact session
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo = homedetails.Logopath;
            HttpContext.Session.SetString("logo", homedetails.Logopath);
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");
            ViewBag.instagram = HttpContext.Session.GetString("instagram");
            ViewBag.facebook = HttpContext.Session.GetString("facebook");
            ViewBag.companyemail = HttpContext.Session.GetString("companyemail");
            ViewBag.phonenumber = HttpContext.Session.GetString("phonenumber");
            ///user session
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            if (id == null || _context.Useraccounts == null)
            {
                return NotFound();
            }

            var customer = await _context.Useraccounts.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            customer.ImagePath = HttpContext.Session.GetString("Image");

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInfo(decimal id, [Bind("Id,Fname,Lname,ImagePath,ImageFile")] Useraccount useraccount, string? Email, string? Currentpassword, string? newpassword, string? confirmpassword)
        {

            HttpContext.Session.SetString("Fname", useraccount.Fname);
            ///contact session
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo = homedetails.Logopath;
            HttpContext.Session.SetString("logo", homedetails.Logopath);
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");
            ViewBag.instagram = HttpContext.Session.GetString("instagram");
            ViewBag.facebook = HttpContext.Session.GetString("facebook");
            ViewBag.companyemail = HttpContext.Session.GetString("companyemail");
            ViewBag.phonenumber = HttpContext.Session.GetString("phonenumber");
            ///user session
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.image = HttpContext.Session.GetString("Image");
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.roleid = HttpContext.Session.GetInt32("roleId");
            var email = HttpContext.Session.GetString("Email");
            var pass = HttpContext.Session.GetString("Password");
            var roleid = HttpContext.Session.GetInt32("roleId");
            var Id = HttpContext.Session.GetInt32("UserId");

            useraccount.ImagePath = HttpContext.Session.GetString("Image");


            if (id != useraccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userlogin = _context.Userlogins.Where(x => x.UserId == id && x.Email == email).FirstOrDefault();

                    if (newpassword != null && Currentpassword != null)
                    {

                        if (Currentpassword != pass)
                        {
                            ViewData["checkcurr"] = "Wrong password";
                            return View(useraccount);

                        }
                        if (newpassword == Currentpassword)
                        {
                            ViewData["checknew"] = "change to different password";
                            return View(useraccount);

                        }

                        userlogin.Passwordd = newpassword;
                        HttpContext.Session.SetString("Password", newpassword);
                        _context.Update(userlogin);
                        await _context.SaveChangesAsync();


                    }
                    else if (newpassword == null && Currentpassword != null)
                    {
                        ViewData["new"] = "write a new password";
                        return View(useraccount);

                    }
                    else if (newpassword != null && Currentpassword == null)
                    {
                        ViewData["current"] = "write the current password";
                        return View(useraccount);

                    }

                    if (Email != email && Email != null)
                    {
                        userlogin.Email = Email;
                        HttpContext.Session.SetString("Email", Email);
                        _context.Update(userlogin);
                        await _context.SaveChangesAsync();
                    }


                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (useraccount.ImageFile != null)
                    {
                        var im = await _context.Useraccounts.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
                        if (im != null)
                        {
                            string path1 = Path.Combine(wwwRootPath + "/Images/" + im.ImagePath);
                            Console.Write(path1);
                            //System.IO.File.Delete(path1);
                        }
                        string fileName = Guid.NewGuid().ToString() + useraccount.ImageFile.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await useraccount.ImageFile.CopyToAsync(fileStream);
                        }

                        useraccount.ImagePath = fileName;
                        _context.Update(useraccount);
                        await _context.SaveChangesAsync();

                        HttpContext.Session.SetString("Image", useraccount.ImagePath);


                    }

                    else
                    {
                        _context.Update(useraccount);
                        _context.Entry(useraccount).Property(x => x.ImagePath).IsModified = false;
                        await _context.SaveChangesAsync();

                    }




                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(useraccount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DisplayInfo));
            }
            return View(useraccount);
        }



        private bool CustomerExists(decimal id)
        {
            return (_context.Useraccounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }



    }
}

