using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FiveMinutes.Models;

public class Answer
{
    [Key]
    public int Id { get; set; }
    public int Position { get; set; }
    public string? AnswerText { get; set; }
    [ForeignKey("EducationTest")]
    public int? QuestionId { get; set; }
}