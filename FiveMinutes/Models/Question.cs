using FiveMinutes.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveMinutes.Models;

public class Question
{
    [Key]
    public int Id { get; set; }
    public int Position { get; set; }
        
    [Required(ErrorMessage = "Текст вопроса обязателен")]
    public string QuestionText { get; set; }
    public ResponseType ResponseType { get; set; }
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [ForeignKey("FiveMinuteTemplate")]
    public int FiveMinuteTemplateId { get; set; }
    public FiveMinuteTemplate FiveMinuteTemplate { get; set; }
}