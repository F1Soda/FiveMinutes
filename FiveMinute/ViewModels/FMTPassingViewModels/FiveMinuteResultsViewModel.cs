using FiveMinute.Models;

namespace FiveMinute.ViewModels;

public class FiveMinuteResultsViewModel
{
    public ICollection<FiveMinuteTestResult> Results = new List<FiveMinuteTestResult>();
}