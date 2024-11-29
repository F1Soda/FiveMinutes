using FiveMinute.Models;
using FiveMinutes.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveMinutes.Models
{
	public class FiveMinuteTest
	{
		[Key]
		public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("FiveMinuteTemplate")]
        public int? AttachedFMTId {  get; set; }
		public FiveMinuteTemplate AttachedFMT { get; set; }
		public TestStatus Status { get; set; }

		public bool StartPlanned = false;
		public DateTime StartTime { get; set; }
		public bool EndPlanned = false;
		public DateTime EndTime { get; set; }

		public IEnumerable<FiveMinuteResult> Results { get; set; }

		[ForeignKey("AppUser")]
		public string? UserOrganizerId { get; set; }
		public AppUser? UserOrganizer { get; set; }
	}
}
