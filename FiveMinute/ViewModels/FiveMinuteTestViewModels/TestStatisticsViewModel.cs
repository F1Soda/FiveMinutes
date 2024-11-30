namespace FiveMinute.ViewModels.FiveMinuteTestViewModels
{
    public class TestStatisticsViewModel
    {
        public int TotalAttempts { get; set; }
        public string TestName { get; set; }
        public ICollection<TestAttemptViewModel> Attempts { get; set; }
    }

    public class TestAttemptViewModel
    {
        public string UserName { get; set; }
        public DateTime PassTime { get; set; }
        public ICollection<AttemptAnswerViewModel> Answers { get; set; }
        public int CorrectAnswersCount => Answers?.Count(a => a.IsCorrect) ?? 0;
    }

    public class AttemptAnswerViewModel
    {
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionPosition { get; set; }
    }
} 