using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class FiveMinuteTestResultViewModel: IInput<FiveMinuteTestResultViewModel,FiveMinuteTest>
{
    public FiveMinuteTestResult FiveMinuteTestResult;
    public string FiveMinuteTestName;
    public ICollection<Question> Questions = new List<Question>();
    public static FiveMinuteTestResultViewModel CreateByModel(FiveMinuteTest model)
    {
       return new FiveMinuteTestResultViewModel
        {
            FiveMinuteTestName = model.Name,
            Questions = model.FiveMinuteTemplate.Questions.ToList()
        };
    }
}