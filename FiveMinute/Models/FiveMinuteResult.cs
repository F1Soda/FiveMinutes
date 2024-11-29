using System.ComponentModel.DataAnnotations;

namespace FiveMinute.Models;

public class FiveMinuteResult
{
    [Key]
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string UserName { get; set; }
    public int FiveMinuteTemplateId { get; set; }
    public FiveMinuteTemplate FiveMinuteTemplate { get; set; }
    public IEnumerable<UserAnswer> Answers { get; set; }
    public DateTime PassTime { get; set; }
}