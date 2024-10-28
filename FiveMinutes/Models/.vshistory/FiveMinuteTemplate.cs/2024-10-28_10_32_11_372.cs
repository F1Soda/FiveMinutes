using FiveMinutes.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
namespace FiveMinutes.Models
{
	public class FiveMinuteTemplate
	{
		[Key]
		public int Id { get; set; }
		public string? Name { get; set; }

		public DateTime? CreationTime { get; set; }
		public DateTime? LastModificationTime { get; set; }
		public ICollection<Question> Questions { get; set; }
        public bool? ShowInProfile { get; set; }

        [ForeignKey("AppUser")]
        public int? UserOwnerId { get; set; }
	}

}
