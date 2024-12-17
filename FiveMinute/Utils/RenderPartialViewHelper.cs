using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Utils
{
	public class RenderPartialViewHelper : Controller
	{
		private readonly ICompositeViewEngine viewEngine;

		public RenderPartialViewHelper(ICompositeViewEngine viewEngine)
		{
			this.viewEngine = viewEngine;
		}
	}
}
