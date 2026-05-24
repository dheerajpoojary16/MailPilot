using BulkMailSender.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BulkMailSender.ViewModels
{
    public class EmailViewModel
    {
        [Required]
        public string Emails { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required] 
        public string Body { get; set; }
        public IFormFile Attachment { get; set; }
        public int SelectedTemplateId { get; set; }
        public List<EmailTemplate>? Templates { get; set; }

    }
}
