using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FiveMinutes.Models;

public abstract class Answer
{
    [Key]
    public int Id { get; set; }
    public int Position { get; set; }
    [ForeignKey("Question")]
    public int QuestionId { get; set; }
    public Question Question { get; set; }
}