using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FiveMinutes.Models
{
	public class AppUser : IdentityUser
	{
		[Key]
		public string Id { get; set; }

	}
}
