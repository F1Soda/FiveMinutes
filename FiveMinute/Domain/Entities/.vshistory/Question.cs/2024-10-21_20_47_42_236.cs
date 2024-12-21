using FiveMinutes.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveMinutes.Models;

public class Question
{
    [Key]
    public int Id { get; set; }
    public int Position { get; set; }
    public string QuestionText { get; set; }
    public ResponseType ResponseType { get; set; }
    public ICollection<SingleChoiceAnswer> SingleChoiceAnswers { get; set; }
    public ICollection<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }
    public ICollection<TextAnswer> TextAnswers { get; set; }

    [ForeignKey("FiveMinuteTemplate")]
    public int? FiveMinuteTemplateId { get; set; }
    public FiveMinuteTemplate FiveMinuteTemplate { get; set; }
}