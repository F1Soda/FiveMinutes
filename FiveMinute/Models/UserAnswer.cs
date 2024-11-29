using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveMinute.Models;

public class UserAnswer
{
    [Key]
    public int Id { get; set; }

    public string Text { get; set; } = "";
    public int Position {get; set;}
    public bool IsCorrect { get; set; }

    [ForeignKey("Question")]
    public int QuestionId { get; set; }

    public string QuestionText { get; set; } = "";

    public int QuestionPosition { get; set; }
}