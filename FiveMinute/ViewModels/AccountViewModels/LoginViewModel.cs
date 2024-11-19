using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace FiveMinutes.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        // Validation Annotation какой-то
        [Display(Name = "EmailAddress Address")]
        [Required(ErrorMessage = "EmailAddress address is required")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
