using DECKMASTER.Data;
using DECKMASTER.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using DECKMASTER.Repositories;
using DECKMASTER.ViewModels;



namespace DECKMASTER.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;



        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var adminUserName = _configuration["adminLogin:Username"];
            var adminPassword = _configuration["AdminLogin:Password"];
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];

            //Session variable for name
            string customerName = "";
            customerName = _db.MyRegisteredUsers.FirstOrDefault(c => c.Email == User.Identity.Name)?.FirstName;
            if(customerName == null)
            {
                customerName = "";
            }
            HttpContext.Session.SetString("SessionUserName", customerName);

            return View();
        }

        public IActionResult PayPalConfirmation(PayPalConfirmation payPalConfirmationModel)
        {

            IPN iPN = new IPN();
            iPN.custom = "";
            iPN.create_time = DateTime.Now.ToLongDateString();
            iPN.paymentID = payPalConfirmationModel.TransactionId;
            iPN.amount = payPalConfirmationModel.Amount;

            iPN.payerLastName = payPalConfirmationModel.PayerName;
            iPN.cart = " ";

            iPN.payerID = " ";
            iPN.payerFirstName = " ";
            iPN.payerMiddleName = " ";
            iPN.payerEmail = payPalConfirmationModel.PayerEmail;
            iPN.payerCountryCode = " ";
            iPN.payerStatus = " ";

            iPN.currency = " ";
            iPN.intent = " ";
            iPN.paymentMethod = "PayPal";
            iPN.paymentState = " ";

            try
            {
                _db.IPNs.Add(iPN);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }


            return View(payPalConfirmationModel);
        }


        public IActionResult Products()
        {
            ProductRepo pRepo = new ProductRepo(_db);
            
            return View(pRepo.GetAllProducts());
        }


        public IActionResult Transactions()
        {
            DbSet<IPN> items = _db.IPNs;

            return View(items);
        }


        //[HttpPost]
        //public JsonResult PaySuccess([FromBody] IPN ipn)
        //{
        //    try
        //    {
        //        _db.IPNs.Add(ipn);
        //        _db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message);
        //    }
        //    return Json(ipn);
        //}

        //public IActionResult Confirmation(string confirmationId)
        //{
        //    IPN transaction =
        //    _db.IPNs.FirstOrDefault(t => t.paymentID == confirmationId);

        //    return View("Confirmation", transaction);
        //}


    }
}