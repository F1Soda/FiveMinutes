using System.ComponentModel.DataAnnotations;

namespace FiveMinute.Models;

public class CompletedQuestion
{
    [Key]
    public int Id { get; set; }
    public int QuestionId { get; set; }
}