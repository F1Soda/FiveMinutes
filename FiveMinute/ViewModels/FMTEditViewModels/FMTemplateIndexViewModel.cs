using FiveMinute.Models;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;

namespace FiveMinute.ViewModels.FMTEditViewModels
{
	public class FMTemplateIndexViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public static FMTemplateIndexViewModel CreateByModel(FiveMinuteTemplate model)
		{
			return new FMTemplateIndexViewModel
			{
				Id = model.Id,
				Name = model.Name,
			};
		}
	}
}
