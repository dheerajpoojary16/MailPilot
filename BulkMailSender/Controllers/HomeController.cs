using Microsoft.AspNetCore.Mvc;
using BulkMailSender.ViewModels;
using MailKit.Net.Smtp;
using MimeKit;
using BulkMailSender.Models;
using BulkMailSender.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace BulkMailSender.Controllers
{
    public class HomeController : Controller   
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        [HttpGet]
        public IActionResult Index()
        {
            EmailViewModel vm = new EmailViewModel();

            vm.Templates = _context.EmailTemplates.ToList();

            return View(vm);
        }
        [HttpGet]
        public JsonResult GetTemplate(int id)
        {
            var template = _context.EmailTemplates
                                   .FirstOrDefault(x => x.Id == id);

           return Json(new
{
    sub = template.sub,
    body = template.body,
    attachment = template.AttachmentPath
});
        }

        [HttpPost]
        public async Task<IActionResult>  Send(EmailViewModel model)
        {
            try
            {
                var emailList = model.Emails.Split(',')
                                            .Select(e => e.Trim())
                                            .Where(e => !string.IsNullOrWhiteSpace(e))
                                            .Distinct(StringComparer.OrdinalIgnoreCase)
                                            .ToList();

                foreach (string email in emailList)
                {
                    string templateAttachment = Request.Form["TemplateAttachment"]; 
                    var message = new MimeMessage();

                    var senderName = _configuration["EmailSettings:SenderName"];
                    var senderEmail = _configuration["EmailSettings:SenderEmail"];
                    var senderPassword = _configuration["EmailSettings:SenderPassword"];
                    var smtpServer = _configuration["EmailSettings:SmtpServer"];
                    var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

                    message.From.Add(
                        new MailboxAddress(
                            senderName,
                            senderEmail
                        )
                    );

                    message.To.Add(
                        MailboxAddress.Parse(email)
                    );

                    message.Subject = model.Subject;

                    var builder = new BodyBuilder();

                    builder.TextBody = model.Body;

                    if (model.Attachment != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            model.Attachment.CopyTo(stream);

                            builder.Attachments.Add(
                                model.Attachment.FileName,
                                stream.ToArray()
                            );
                        }
                    }
                    else if (!string.IsNullOrEmpty(templateAttachment))
                    {
                        string fullPath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            templateAttachment
                        );

                        builder.Attachments.Add(
    "Deeeraj_Resume.pdf",
    System.IO.File.ReadAllBytes(fullPath)
);
                    }

                    message.Body = builder.ToMessageBody();

                    using (var client = new SmtpClient())
                    {
                        client.Connect(
                            smtpServer,
                            smtpPort,
                            true
                        );

                        client.Authenticate(
                            senderEmail,
                            senderPassword
                        );

                        client.Send(message);

                        client.Disconnect(true);
                    }
                }

                return Json(new { success = true, message = "Emails sent successfully!" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
