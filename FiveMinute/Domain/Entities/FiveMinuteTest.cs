using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FiveMinute.Data;
using FiveMinute.Domain.FMTestChecker;

namespace FiveMinute.Models;

public class FiveMinuteTest
{
	[Key]
	public int Id { get; set; }
	public string Name { get; set; }

	[ForeignKey("FiveMinuteTemplate")]
	public int? FiveMinuteTemplateId { get; set; }
	public FiveMinuteTemplate FiveMinuteTemplate { get; set; }
	public TestStatus Status { get; set; }

	public DateTime CreationTime { get; set; }
	
	public bool StartPlanned { get; set; }
	public DateTime StartTime { get; set; }
	public bool EndPlanned { get; set; }
	public DateTime EndTime { get; set; }

	public List<int> IdToUninclude { get; set; }
	public List<FiveMinuteTestResult> Results { get; set; }

	[ForeignKey("AppUser")]
	public string? UserOrganizerId { get; set; }
	public AppUser? UserOrganizer { get; set; }

	public bool CanPass() => PassChecker.CanPass(this);
	
}