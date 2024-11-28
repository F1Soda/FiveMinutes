using FiveMinutes.Data;
using Microsoft.AspNetCore.Identity;

namespace FiveMinutes.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<FiveMinuteTemplate> FMTs { get; set; }
        public ICollection<FiveMinuteResult> Result { get; set; }

        public string UserRole {  get; set; }
        public bool canCreate => UserRole is UserRoles.Admin or UserRoles.Teacher; 

        public void AddFMT(FiveMinuteTemplate fmt)
        {
            FMTs.Add(fmt);
        }
    }
}
