using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FiveMinutes.Models;

public class FiveMinuteResult
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    [MaxLength(30)]
    public string? UserName { get; set; }
    public int FiveMinuteTemplateId { get; set; }
    public List<UserAnswer> Answers { get; set; } = new();
    public DateTime PassTime { get; set; }
}