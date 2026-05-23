using Microsoft.AspNetCore.Mvc;
using BulkMailSender.ViewModels;
using System.Net.Mail;
using System.Net;
namespace BulkMailSender.Controllers
{
    public class HomeController : Controller   
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendEmail(EmailViewModel model)
        {
          
            return View("Index");
        }
    }
}
