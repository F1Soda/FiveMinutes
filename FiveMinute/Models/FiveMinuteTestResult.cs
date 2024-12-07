using FiveMinute.Data;
using System.ComponentModel.DataAnnotations;

namespace FiveMinute.Models;

public class FiveMinuteTestResult
{
	[Key]
	public int Id { get; set; }
	public string? UserId { get; set; }
	public string? UserName { get; set; }
	public UserData StudentData { get; set; }
	public ResultStatus Status { get; set; }
	public int FiveMinuteTestId { get; set; }
	public IEnumerable<UserAnswer> Answers { get; set; }
	public DateTime PassTime { get; set; }
}