using System.ComponentModel.DataAnnotations;
using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FMTEditViewModels
{
    public class AnswerEditViewModel: IInput<AnswerEditViewModel,Answer>,IOutput<AnswerEditViewModel,Answer>
    {
        public int Position { get; set; }
        [Required(ErrorMessage = "Текст ответа обязателен")]
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public static AnswerEditViewModel CreateByModel(Answer model)
        {
            return new AnswerEditViewModel
            {
                Position = model.Position,
                Text = model.Text,
                IsCorrect = model.IsCorrect
            };
        }

        public static Answer CreateByView(AnswerEditViewModel model)
        {
            return new Answer
            {
                Position = model.Position,
                Text = model.Text,
                IsCorrect = model.IsCorrect
            };
        }
    }
}
