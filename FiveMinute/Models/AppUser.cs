using FiveMinutes.Data;
using Microsoft.AspNetCore.Identity;

namespace FiveMinutes.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<FiveMinuteTemplate> FMTTemplates { get; set; }
        public ICollection<FiveMinuteTest> FMTests { get; set; }
        public ICollection<FiveMinuteResult> Result { get; set; }

        public string UserRole {  get; set; }
        public bool canCreate => UserRole is UserRoles.Admin or UserRoles.Teacher; 

        public void AddFMT(FiveMinuteTemplate fmt)
        {
            FMTTemplates.Add(fmt);
        }
    }
}
