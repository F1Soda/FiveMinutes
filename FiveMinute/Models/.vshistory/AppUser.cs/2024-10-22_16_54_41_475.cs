using Microsoft.AspNetCore.Identity;

namespace FiveMinutes.Models
{
	public class AppUser : IdentityUser
	{
        public ICollection<FiveMinuteTemplate> FMTs { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
