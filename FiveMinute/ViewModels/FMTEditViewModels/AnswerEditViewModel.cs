using System.ComponentModel.DataAnnotations;

namespace FiveMinute.ViewModels.FMTEditViewModels
{
    public class AnswerEditViewModel
    {
        public int Position { get; set; }
        [Required(ErrorMessage = "Текст ответа обязателен")]
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
