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
<<<<<<< HEAD
=======
		
		private async Task<string> RenderPartialViewToString(string viewName, object model)
		{
			ViewData.Model = model;

			using (var sw = new StringWriter())
			{
				var viewResult = viewEngine.FindView(ControllerContext, viewName, false);
				if (!viewResult.Success)
				{
					throw new InvalidOperationException($"Could not find view: {viewName}");
				}

				var viewContext = new ViewContext(
					ControllerContext,
					viewResult.View,
					ViewData,
					TempData,
					sw,
					new HtmlHelperOptions()
				);
				await viewResult.View.RenderAsync(viewContext);
				return sw.GetStringBuilder().ToString();
			}
		}
>>>>>>> origin/cynpy
	}
}
