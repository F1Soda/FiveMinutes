using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
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

        public static IHtmlString SerializeObject(object value)
        {
			using (var stringWriter = new StringWriter())
			using (var jsonWriter = new JsonTextWriter(stringWriter))
			{
			}
    }
}
