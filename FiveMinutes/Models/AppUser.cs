using Microsoft.AspNetCore.Identity;

namespace FiveMinutes.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<FiveMinuteTemplate> FMTs { get; set; }
        public ICollection<FiveMinuteTest> Tests { get; set; }

        public string UserRole {  get; set; }  

        public void AddFMT(FiveMinuteTemplate fmt)
        {
            FMTs.Add(fmt);
        }
    }
}
