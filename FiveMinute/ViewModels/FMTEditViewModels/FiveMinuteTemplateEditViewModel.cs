using FiveMinute.Models;

namespace FiveMinute.ViewModels.FMTEditViewModels
{
    public class FiveMinuteTemplateEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionEditViewModel> Questions { get; set; }
        public bool ShowInProfile { get; set; }


        public static FiveMinuteTemplateEditViewModel CreateByModel(FiveMinuteTemplate fmt)
        {
            return new FiveMinuteTemplateEditViewModel
            {
                Id = fmt.Id,
                Name = fmt.Name,
                ShowInProfile = fmt.ShowInProfile,
                Questions = fmt.Questions
                    .Select(x => new QuestionEditViewModel()
                    {
                        QuestionText = x.QuestionText,
                        Position = x.Position,
                        ResponseType = x.ResponseType,
                        Answers = x.AnswerOptions.Select(x => new AnswerEditViewModel
                        {
                            Position = x.Position,
                            Text = x.Text,
                            IsCorrect = x.IsCorrect
                        }).ToList()
                    }).ToList()
            };
        }
        public FiveMinuteTemplate CreateByView()//надо кому-то это дажать у меня сил не хватило но идея класс
        {
            return new FiveMinuteTemplate
            {
                Id = this.Id,
                Name = this.Name,
                ShowInProfile = this.ShowInProfile,
                Questions = this.Questions
                    .Select(x => new Question()
                    {
                        QuestionText = x.QuestionText,
                        Position = x.Position,
                        ResponseType = x.ResponseType,
                        AnswerOptions = x.Answers.Select(x => new Answer()
                        {
                            Position = x.Position,
                            Text = x.Text,
                            IsCorrect = x.IsCorrect
                        }).ToList()
                    }).ToList()
            };
        }
    }
}
