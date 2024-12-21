using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveMinutes.Models
{
	public class Test
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("AppUser")]
		public int? AppUserId { get; set; }



	}
}
