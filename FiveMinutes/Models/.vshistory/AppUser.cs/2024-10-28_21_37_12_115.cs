using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web;

namespace FiveMinutes.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<FiveMinuteTemplate> FMTs { get; set; }
        public ICollection<FiveMinuteTest> Tests { get; set; }

        public void AddFMT(FiveMinuteTemplate fmt)
        {
            FMTs.Add(fmt);
        }
    }
}
