using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace FiveMinute.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "EmailAddress Address")]
        [Required(ErrorMessage = "EmailAddress address is required")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
