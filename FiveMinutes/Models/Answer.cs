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

public class SingleChoiceAnswer : Answer
{
    public bool IsSelected { get; set; }
}

public class MultipleChoiceAnswer : Answer
{
    public bool IsChecked { get; set; }
}

public class TextAnswer : Answer
{
    public string Text { get; set; }
}