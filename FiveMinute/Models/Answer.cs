using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FiveMinute.Models;

public class Answer
{
    [Key]
    public int Id { get; set; }
    public int Position { get; set; }
        
    [Required(ErrorMessage = "Текст ответа обязателен")]
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    [ForeignKey("Question")]
    public int QuestionId { get; set; }
}

public enum ResponseType
{
    SingleChoice,
    MultipleChoice,
    Text
}