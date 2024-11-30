using FiveMinute.Models;

namespace FiveMinute.ViewModels;

public class FiveMinuteTestResultViewModel
{
    public FiveMinuteTestResult FiveMinuteTestResult;
    public string FiveMinuteTestName;
    public ICollection<Question> Questions = new List<Question>();
}