using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FiveMinutes.Models
{
	public class FiveMinuteTemplate
	{
		[Key]
		public string Name { get; set; }

		public ICollection<Question> Questions { get; set; }
        public bool ShowInProfile = true;

        [ForeignKey("AppUser")]
        public int? UserOwnerId { get; set; }
	}
}
