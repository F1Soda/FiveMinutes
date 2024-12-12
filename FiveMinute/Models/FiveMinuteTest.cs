using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FiveMinute.Data;

namespace FiveMinute.Models
{
	public class FiveMinuteTest
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		[ForeignKey("FiveMinuteTemplate")]
		public int? FiveMinuteTemplateId { get; set; }
		public FiveMinuteTemplate FiveMinuteTemplate { get; set; }
		public TestStatus Status { get; set; }

		public bool StartPlanned = false;
		public DateTime StartTime { get; set; }
		public bool EndPlanned = false;
		public DateTime EndTime { get; set; }

		public List<int> IdToUninclude { get; set; }
		public List<FiveMinuteTestResult> Results { get; set; }

		[ForeignKey("AppUser")]
		public string? UserOrganizerId { get; set; }
		public AppUser? UserOrganizer { get; set; }
	}
}