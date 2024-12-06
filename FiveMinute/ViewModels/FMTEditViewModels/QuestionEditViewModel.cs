using FiveMinute.Models;

namespace FiveMinute.ViewModels.FMTEditViewModels
{
    public class QuestionEditViewModel
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string QuestionText { get; set; }
        public ResponseType ResponseType { get; set; }
        public ICollection<AnswerEditViewModel> Answers { get; set; } = new List<AnswerEditViewModel>();
    }
}
