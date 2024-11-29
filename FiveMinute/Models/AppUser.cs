using FiveMinute.Data;
using Microsoft.AspNetCore.Identity;

namespace FiveMinute.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<FiveMinuteTemplate> FMTemplates { get; set; }
        public ICollection<FiveMinuteTest> FMTests { get; set; }
        public ICollection<int> PassedFMTestIds { get; set; }

        public string UserRole {  get; set; }
        public bool canCreate => UserRole is UserRoles.Admin or UserRoles.Teacher; 

        public void AddFMT(FiveMinuteTemplate fmTemplate)
        {
            FMTemplates.Add(fmTemplate);
        }
    }
}
