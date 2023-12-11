using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyTransformer.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Org.BouncyCastle.Bcpg;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Image = iTextSharp.text.Image;

namespace MoneyTransformer.Controllers
{
    public class UserController : Controller
    {
        private readonly ModelContext _context;
        private static Random rnd1 = new Random();
        public static int Flag;
        public UserController(ModelContext context)
        {
            _context = context;
             Flag = 0;
        }
        public IActionResult Home()
        {
            HttpContext.Session.Remove("review");
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
            var userid = HttpContext.Session.GetInt32("UserId");

            var testimonial = _context.Testimonials.Include(t => t.User);
            var home = _context.Homes.ToList();
            var wal = _context.Wallets.Where(x => x.UserId == userid && x.BankId!=null).Include(t => t.Bank).ToList();
            var banks = _context.Banks.ToList();
            var modelContext = Tuple.Create<IEnumerable<Testimonial>, IEnumerable<Home>, IEnumerable<Wallet>>(testimonial, home, wal);
            return View(modelContext);
        }
     
        // GET: Testimonials/Create
        public IActionResult AddReview()
        {
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

            ViewData["review"] = HttpContext.Session.GetString("review");
            
            // ViewData["UserId"] = new SelectList(_context.Useraccounts, "Id", "Id");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview([Bind("Id,Text,Status,TestimonialDate,UserId")] Testimonial testimonial)
        {
            
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


            if(testimonial.Text == null)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                Flag = 1;
                HttpContext.Session.SetString("review", "Thank your for sharing your experience");
               
                testimonial.Status = "onhold";
                testimonial.UserId= HttpContext.Session.GetInt32("UserId");
                testimonial.TestimonialDate = DateTime.Now;
                _context.Add(testimonial);
                await _context.SaveChangesAsync();

             
            }


            //ViewData["UserId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UserId);
            return RedirectToAction(nameof(AddReview));
        }

        // GET: Wallets/Create
        public async Task<IActionResult> CreateWallet()
        {
            HttpContext.Session.Remove("review");
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
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");
            var userid = HttpContext.Session.GetInt32("UserId"); 
            ViewData["BankId"] = new SelectList(_context.Banks, "Id", "Namee");
            ViewData["UserId"] = new SelectList(_context.Useraccounts, "Id", "Id");

            var wallet = _context.Wallets.Where(x => x.UserId == userid).ToList();

          
            var banks = _context.Banks.ToList();
            var modelContext = Tuple.Create < IEnumerable <Wallet>,IEnumerable<Bank>>(wallet, banks);
            return View(modelContext);
        

            //return _context.Banks != null ?
            //              View(await _context.Banks.ToListAsync()) :
            //              Problem("Entity set 'ModelContext.Banks'  is null.");
            //return View();
        }

        // POST: Wallets/Create
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWallet(decimal? id,string ? name)
        {
            HttpContext.Session.Remove("review");
            Wallet wallet = new Wallet();
            wallet.BankId = id;
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
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");
           var userid = HttpContext.Session.GetInt32("UserId");
            if (name!=null)

            {
                var wallet2 = _context.Wallets.Where(x => x.UserId == userid).ToList();

                var banks = _context.Banks.ToList();
                var re = _context.Banks.Where(x => x.Namee.Contains(name) || x.Namee.ToLower().Contains(name)||x.Namee.ToUpper().Contains(name)).ToList();
                
                var model2 = Tuple.Create<IEnumerable<Wallet>, IEnumerable<Bank>>(wallet2, re);

                return View(model2);
             
            }
            if (id != null)
            {
                
                if (ModelState.IsValid)
                {
                    var userwallet = _context.Wallets.Where(x => x.BankId == wallet.BankId && x.UserId == userid).FirstOrDefault();
                    if (userwallet == null)
                    {
                        wallet.Status = "notActive";
                        wallet.Balance = 0;
                        wallet.UserId = userid;
                        wallet.Datecreate = DateTime.Now;


                        wallet.Iban = rnd1.NextInt64(1000000000, 9999999999);

                        _context.Add(wallet);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("WalletDetails", "User", new { id = wallet.Id });
                    }
                    else
                    {
                        ViewData["bankcheck"] = "You have already account in this bank";
                    }
                }
            }
            ViewData["BankId"] = new SelectList(_context.Banks, "Id", "Namee", wallet.BankId);
            ViewData["UserId"] = new SelectList(_context.Useraccounts, "Id", "Id", wallet.UserId);
            return RedirectToAction(nameof(CreateWallet));
            
        }
        [HttpPost]
      
        public IActionResult CWallet(string? name,int f)
        {
            HttpContext.Session.Remove("review");
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
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");
            var userid = HttpContext.Session.GetInt32("UserId");
            
            var wallet = _context.Wallets.Where(x => x.UserId == userid).ToList();

            var banks = _context.Banks.ToList();
            var model = Tuple.Create<IEnumerable<Wallet>, IEnumerable<Bank>>(wallet, banks);
            //var model = _context.ProductCustomers.Include(x => x.Product).Include(x => x.Customer).ToList();

            //if (name != null)
            //{
            //    var result = from b in banks where b.Namee.Contains(name); 
            //    //var result = banks.Where(x => x.Namee == name);
            //    var model2 = Tuple.Create<IEnumerable<Wallet>, IEnumerable<Bank>>(wallet, result);

            //    return View(model2);
            //}
            return View(model);

        }
        // GET: VirtualBanks/Create
        public IActionResult RechangeActivate(decimal? id)
        {
            HttpContext.Session.Remove("review");
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
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: VirtualBanks/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RechangeActivate(decimal id, [Bind("Cardnumber,Expireddate,Cvv")] VirtualBank virtualBank,decimal amount)
        {
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
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");
            var userid = HttpContext.Session.GetInt32("UserId");

            if (ModelState.IsValid)
            {

                
                var virtualBanks = _context.VirtualBanks.Where
                    (x => x.Cardnumber == virtualBank.Cardnumber && x.Expireddate==virtualBank.Expireddate && x.Cvv==virtualBank.Cvv).FirstOrDefault();

               


                if (virtualBanks == null)
                {
                    ViewData["notvalid"] = "not Found Visa Card with this information";
                    return View(virtualBank);
                }
                else
                {
                    if(virtualBanks.Expireddate < DateTime.Now)
                    {
                        ViewData["notvalid"] = "Visa Card is Expired";
                        return View(virtualBank);
                    }
                    if(virtualBanks.Balance <= amount)
                    {
                        ViewData["notvalid"] = "Visa Card Doesnt have enoughe balance";
                        return View(virtualBank);
                    }
                    else
                    {
                        

                        virtualBanks.Balance = virtualBanks.Balance - amount;
                        _context.Update(virtualBanks);
                        await _context.SaveChangesAsync();
                        var wallet = _context.Wallets.Where(x => x.UserId == userid && x.Id == id).FirstOrDefault();

                        var walletBank = _context.Banks.Where(x => x.Id == wallet.BankId).FirstOrDefault();

                        var s = wallet.Status;

                        wallet.Balance = wallet.Balance+amount;
                        wallet.Status = "Active";
                        _context.Update(wallet);
                        await _context.SaveChangesAsync();
                        ////
                        // Create a new PDF document
                        Document doc = new Document();
                        string pdfFilePath = "E:\\C#\\MVC\\Tasneem_ALshorman\\pdf\\Wallet is Activated"+wallet.Iban+".pdf";

                        // Create a PDF writer to write content to the document
                        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));

                        doc.Open();
                        string imagePath = "E:\\C#\\MVC\\Tasneem_ALshorman\\MoneyTransformer\\MoneyTransformer\\wwwroot\\Images\\logoblue.png";
                        Image img = Image.GetInstance(imagePath);
                        // Set the desired width and height
                        float desiredWidth = 100;   // Adjust this value to your desired width in points (1 inch = 72 points)
                        float desiredHeight = 50;  // Adjust this value to your desired height in points

                        // Scale the image to fit the specified dimensions
                        img.ScaleToFit(desiredWidth, desiredHeight);

                       // img.Alignment = Element.ALIGN_CENTER; // You can adjust the alignment as needed
                        doc.Add(img);
                        // Add content to the PDF
                        // Create a bold font using the FontFactory class
                        Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                        // Add bold text to the PDF
                        Paragraph paragraph = new Paragraph("\r\n\r\nAccount Activation Confirmation and Invoice.", boldFont);
                        doc.Add(paragraph);
                        doc.Add(new Paragraph("\r\n\r\nWe are pleased to confirm that your wallet account with MoneyMove has been successfully activated and is now ready for use. To ensure uninterrupted access to our services, kindly settle the corresponding invoice detailed below:"));
                        //paragraph.Add(Chunk.NEWLINE);
                        doc.Add(new Paragraph("\r\n\r\n1) Invoice Date: " + DateTime.Now));
                        doc.Add(new Paragraph("2) Wallet Iban: "+wallet.Iban));
                        doc.Add(new Paragraph("3) Wallet Balance: "+wallet.Balance+"  JD"));
                        doc.Add(new Paragraph("4) Wallet Bank: " + walletBank.Namee));
                        doc.Add(new Paragraph("\r\n\r\nThank you for choosing MoneyMove. We value your partnership and look forward to providing you with exceptional service.\r\n\r\nBest regards,\r\n moneymove664@gmail.com"));
                        doc.Close();
                        writer.Close();

                        ////sending email to the visacard owner
                        var email = new MimeMessage();

                        email.From.Add(new MailboxAddress("MoneyMove Website", "moneymove664@gmail.com"));
                        email.To.Add(new MailboxAddress(virtualBanks.Ownername, virtualBanks.Owneremanil));

                        email.Subject = "Transaction";

                        var b = wallet.Balance + amount;
                        BodyBuilder bodyBuilder = new BodyBuilder();
                        if(s=="Active")
                        {
                            bodyBuilder.TextBody = "Thank you, Transaction done succefully "+ amount + "JD have been withdrawm from your account, and the wallet ("+ walletBank.Namee + ") is recharged,\nyour account Balance now is : "
                                + virtualBanks.Balance+"JD";


                        }
                        else {
                            bodyBuilder.TextBody = "Thank you, Transaction done succefully " + amount + "JD have been withdrawm from your account, and the wallet ("+ walletBank.Namee + ") is activated.\nyour account Balance now is : "
                                + virtualBanks.Balance + "JD";
                           // bodyBuilder.Attachments.Add("C:\\Users\\USER\\Desktop\\training\\MVC\\FirstProject\\Wallet is Activated.pdf");

                            // Attach the PDF file
                           // Attachment attachment = new Attachment(pdfFilePath);
                            bodyBuilder.Attachments.Add(pdfFilePath);


                        }
                        email.Body = bodyBuilder.ToMessageBody();

                        using (var smtp = new SmtpClient())
                        {
                            smtp.Connect("smtp.gmail.com", 587, false);

                            smtp.Authenticate("moneymove664@gmail.com", "rjlbyczfgxiphszx");

                            smtp.Send(email);
                            smtp.Disconnect(true);
                        }
                        return RedirectToAction("WalletDetails","User", new {id = id }); 

                    }

                }
                
               
            }
            return View(virtualBank);
        }


        // GET: Transactions/Create
        public IActionResult Transaction()
        {
            HttpContext.Session.Remove("review");
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
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");
            var userid = HttpContext.Session.GetInt32("UserId");

            var wallet = _context.Wallets.Where(x => x.UserId == userid).ToList();

            var bank = _context.Banks.ToList();
            var join = from w in wallet
                       join b in bank on w.BankId equals b.Id where w.Status == "Active"
                       select new BankWalletJoin { bank = b, Wallet = w };
            ViewData["SwalletId"] = new SelectList(join, "Wallet.Id", "bank.Namee");


            return View();
        }

        // POST: Transactions/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transaction( [Bind("Id,Amount,SwalletId,RIban")] Transaction transaction)
        {
            HttpContext.Session.Remove("review");
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
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");

            var userid = HttpContext.Session.GetInt32("UserId");
            var Email = HttpContext.Session.GetString("Email");
            var Fname = HttpContext.Session.GetString("Fname");

            if (ModelState.IsValid)
            {
               
                var Recwallet = _context.Wallets.Where(x => x.Iban == transaction.RIban).FirstOrDefault();
                var senwallet = _context.Wallets.Where(x => x.Id == transaction.SwalletId).FirstOrDefault();
               

                if (Recwallet == null || Recwallet.Status=="notActive")
                {
                    ViewData["validiban"] = "Sorry Not found Reciever Wallet";
           

                }
                else if (senwallet.Balance > transaction.Amount)
                {

                    var recBank = _context.Banks.Where(x => x.Id == Recwallet.BankId).FirstOrDefault();
                    var senBank = _context.Banks.Where(x => x.Id == senwallet.BankId).FirstOrDefault();

                    if (Recwallet.BankId == senwallet.BankId)
                    {
                        transaction.Commission = 0;
                    }
                    else
                    {
                        transaction.Commission = (decimal).005 * transaction.Amount;
                    }


                    Recwallet.Balance += transaction.Amount;
                    senwallet.Balance = senwallet.Balance - transaction.Amount - transaction.Commission;
                    transaction.TranDate = DateTime.Now;


                    _context.Update(Recwallet);
                    await _context.SaveChangesAsync();
                    _context.Update(senwallet);
                    await _context.SaveChangesAsync();
                    _context.Add(transaction);
                    await _context.SaveChangesAsync();

                    ////sending email to the reciever wallet
                    ///
                    var recAccount = _context.Userlogins.Where(x => x.UserId == Recwallet.UserId).FirstOrDefault();
                    var recuser = _context.Useraccounts.Where(x => x.Id == Recwallet.UserId).FirstOrDefault();

                    var email1 = new MimeMessage();

                    email1.From.Add(new MailboxAddress("MoneyMove Website", "moneymove664@gmail.com"));
                    email1.To.Add(new MailboxAddress(recuser.Fname, recAccount.Email));
                    
                    email1.Subject = "Transaction";

                    BodyBuilder bodyBuilder1 = new BodyBuilder();
                    
                    bodyBuilder1.TextBody ="wallet("+ recBank.Namee + ")\n Your Balance Now is : "
                            + Recwallet.Balance + "JD";

                   
                    email1.Body = bodyBuilder1.ToMessageBody();

                    ///send email to the sender wallet
                    var email2 = new MimeMessage();

                    email2.From.Add(new MailboxAddress("MoneyMove Website", "moneymove664@gmail.com"));
                    email2.To.Add(new MailboxAddress( Fname, Email));

                    email2.Subject = "Transaction";

                    BodyBuilder bodyBuilder2 = new BodyBuilder();

                    bodyBuilder2.TextBody = "The Transaction done succesfully, "+ transaction.Amount+ "JD have been withdrawm from wallet("+ senBank.Namee + ")\n Your Balance Now is :"
                            + senwallet.Balance + "JD";


                    email2.Body = bodyBuilder2.ToMessageBody();

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Connect("smtp.gmail.com", 587, false);

                        smtp.Authenticate("moneymove664@gmail.com", "rjlbyczfgxiphszx");

                        smtp.Send(email1);
                        smtp.Send(email2);
                        smtp.Disconnect(true);
                    }

                    return RedirectToAction(nameof(Transaction));

                }
                else
                {
                    ViewData["validiban"] = "Sorry you Dont have enough balance ";
                  

                }

            }
            var wallet = _context.Wallets.Where(x => x.UserId == userid).ToList();

            var bank = _context.Banks.ToList();
            var join = from w in wallet
                       join b in bank on w.BankId equals b.Id
                       where w.Status == "Active"
                       select new BankWalletJoin { bank = b, Wallet = w };
            ViewData["SwalletId"] = new SelectList(join, "Wallet.Id", "bank.Namee");

            //ViewData["SwalletId"] = new SelectList(_context.Wallets, "Id", "BankId", transaction.SwalletId);
            return View();
        }

        // GET: Wallets/Details/5
        public async Task<IActionResult> WalletDetails(decimal? id)
        {
            HttpContext.Session.Remove("review");
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
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.Bank)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

    }

}

