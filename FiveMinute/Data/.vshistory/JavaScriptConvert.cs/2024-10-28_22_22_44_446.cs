using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FiveMinutes.Data
{
	public static class JavaScriptConvert
	{
		public static HtmlString SerializeObject(object value)
		{
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Formatting = Formatting.Indented // Optional: for pretty-printing JSON
			};

			string json = JsonConvert.SerializeObject(value, settings);
			return new HtmlString(json);
		}
	}
}
