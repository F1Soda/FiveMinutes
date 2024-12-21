using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FMTEditViewModels
{
	public class FMTemplateIndexViewModel : IInput<FMTemplateIndexViewModel,FiveMinuteTemplate>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime lastModification { get; set; }

		public static FMTemplateIndexViewModel CreateByModel(FiveMinuteTemplate model)
		{
			return new FMTemplateIndexViewModel
			{
				Id = model.Id,
				Name = model.Name,
				lastModification = model.LastModificationTime
			};
		}
	}
}
