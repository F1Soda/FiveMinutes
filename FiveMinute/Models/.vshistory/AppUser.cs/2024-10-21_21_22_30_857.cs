using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FiveMinutes.Models
{
	public class AppUser : IdentityUser
	{


        public ICollection<FiveMinuteTemplate> FMTs { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
