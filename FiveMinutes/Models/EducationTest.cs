using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveMinutes.Models
{
	public class EducationTest
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("AppUser")]
		public int? AppUserId { get; set; }
		public ICollection<Answer> Answers { get; set; }
	}
}
