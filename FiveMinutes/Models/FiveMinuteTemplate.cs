using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FiveMinutes.Models
{
	public class FiveMinuteTemplate
	{
		[Key]
		public int Id { get; set; }
        
		[Required(ErrorMessage = "Название обязательно")]
		public string Name { get; set; }

		public DateTime CreationTime { get; set; }
		public DateTime LastModificationTime { get; set; }
		public List<Question> Questions { get; set; } = new List<Question>();
		public bool ShowInProfile { get; set; }

		[ForeignKey("AppUser")]
		public int? UserOwnerId { get; set; }
	}
}
