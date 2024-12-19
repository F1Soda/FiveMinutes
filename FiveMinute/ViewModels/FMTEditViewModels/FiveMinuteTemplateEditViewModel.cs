using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FMTEditViewModels
{
    public class FiveMinuteTemplateEditViewModel: IInput<FiveMinuteTemplateEditViewModel,FiveMinuteTemplate>,IOutput<FiveMinuteTemplateEditViewModel,FiveMinuteTemplate>
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
                    .Select(x => QuestionEditViewModel.CreateByModel(x)).ToList()
            };
        }
        public static FiveMinuteTemplate CreateByView(FiveMinuteTemplateEditViewModel FiveMinuteTemplateView)
        {
            return new FiveMinuteTemplate
            {
                Id = FiveMinuteTemplateView.Id,
                Name = FiveMinuteTemplateView.Name,
                ShowInProfile = FiveMinuteTemplateView.ShowInProfile,
                Questions = FiveMinuteTemplateView.Questions
                    .Select(x => QuestionEditViewModel.CreateByView(x)).ToList()
            };
        }
    }
}
