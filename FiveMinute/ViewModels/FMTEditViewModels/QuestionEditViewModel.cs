using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FMTEditViewModels
{
    public class QuestionEditViewModel : IInput<QuestionEditViewModel,Question>,IOutput<QuestionEditViewModel,Question>
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string QuestionText { get; set; }
        public ResponseType ResponseType { get; set; }
        public ICollection<AnswerEditViewModel> Answers { get; set; } = new List<AnswerEditViewModel>();
        public static QuestionEditViewModel CreateByModel(Question model)
        {
            return new QuestionEditViewModel()
            {
                Id = model.Id,
                QuestionText = model.QuestionText,
                Position = model.Position,
                ResponseType = model.ResponseType,
                Answers = model.AnswerOptions.Select(x => AnswerEditViewModel.CreateByModel(x)).ToList()
            };
        }

        public static Question CreateByView(QuestionEditViewModel model)
        {
           return new Question{
                QuestionText = model.QuestionText,
                Position = model.Position,
                ResponseType = model.ResponseType,
                AnswerOptions = model.Answers.Select(x => AnswerEditViewModel.CreateByView(x)).ToList()
            };
        }
    }
}
