using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace FiveMinutes.ViewModels
{
	public class LoginViewModel
	{
		// Validation Annotation какой-то
		[Display(Name = "Email Address")]
        public string Email{ get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password{ get; set; }
    }
}
