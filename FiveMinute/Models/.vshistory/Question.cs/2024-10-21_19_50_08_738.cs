using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace FiveMinutes.Models;

public class Question
{
    [Key] 
    public int Id { get; set; }
    public int Position { get; set; }
    public string? Text { get; set; }
    public string? CorrectAnswer { get; set; }
    [ForeignKey("FiveMinuteTemplate")]
    public int? FiveMinuteTemplateId { get; set; }
}