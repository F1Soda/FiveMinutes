using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace FiveMinute.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        // Validation Annotation какой-то
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }
        
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; }
        
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
