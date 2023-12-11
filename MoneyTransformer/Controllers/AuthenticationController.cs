using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MoneyTransformer.Models;
using NuGet.Protocol.Plugins;
using System.IO;

namespace MoneyTransformer.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AuthenticationController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Logout()
        {

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Fname,Lname,ImagePath,ImageFile")] Useraccount useraccount, string Email, string password,string compassword)
        {
            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;

            if (password!=compassword)
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
                userLogin.Email= Email;
                userLogin.Passwordd = password;
                userLogin.UserId = useraccount.Id;
                userLogin.RoleId = 2;

                _context.Add(userLogin);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Login));

            }
            return View(useraccount);
        }

        public IActionResult Login()
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;

           

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login([Bind("Id,Email,Passwordd,RoleId,UserId")] Userlogin userLogin)
        {
            ViewBag.logo2 = HttpContext.Session.GetString("logo2");

            var homedetails = _context.Homes.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.logo2 = homedetails.Image2path;

            var auth = _context.Userlogins.Where(x => x.Passwordd == userLogin.Passwordd && x.Email == userLogin.Email).FirstOrDefault();
            ViewBag.flag = 0;
            if (auth != null)
            {
                var customer = _context.Useraccounts.Where(x => x.Id == auth.UserId).FirstOrDefault();
                if (auth.RoleId == 1)
                {
                    HttpContext.Session.SetInt32("UserId", (int)auth.UserId);
                    HttpContext.Session.SetString("Email", auth.Email);
                    HttpContext.Session.SetString("Password", auth.Passwordd);

                    HttpContext.Session.SetString("Fname", customer.Fname);
                    HttpContext.Session.SetString("Lname", customer.Lname);
                    HttpContext.Session.SetInt32("roleId", (int)auth.RoleId);
                    HttpContext.Session.SetString("Image", customer.ImagePath);

                    return RedirectToAction("Index", "Admin");

                }

                else if (auth.RoleId == 2)
                {
                    HttpContext.Session.SetInt32("UserId", (int)auth.UserId);
                    HttpContext.Session.SetString("Email", auth.Email);
                    HttpContext.Session.SetString("Password", auth.Passwordd);
                    HttpContext.Session.SetString("Fname", customer.Fname);
                    HttpContext.Session.SetString("Lname", customer.Lname);
                    HttpContext.Session.SetInt32("roleId", (int)auth.RoleId);

                    HttpContext.Session.SetString("Image", customer.ImagePath);

                    return RedirectToAction("Home", "User");

                    //return RedirectToAction("Index", "Categories");
                }


            }
            else
            {

                ViewData["Messag"] = "Wrong Username or Password";
            }
            return View();
        }


    }
}
