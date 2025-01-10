using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FiveMinute.Data;

namespace FiveMinute.Models;

public class UserAnswer
{
    [Key]
    public int Id { get; set; }

    public string Text { get; set; } = "";
    public int Position {get; set;}
    public bool IsCorrect { get; set; }
    public float Score { get; set; }

	[ForeignKey("Question")]
    public int QuestionId { get; set; }

    public string QuestionText { get; set; } = "";

    public int QuestionPosition { get; set; }
    
    public ResultStatus ResultStatus { get; set; }
}