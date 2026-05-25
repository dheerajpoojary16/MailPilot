using System.ComponentModel.DataAnnotations;

namespace BulkMailSender.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
