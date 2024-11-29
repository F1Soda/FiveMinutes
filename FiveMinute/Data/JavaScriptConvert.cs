using Microsoft.AspNetCore.Html;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace FiveMinute.Data
{
	public static class JavaScriptConvert
	{
		public static HtmlString SerializeObject(object value)
		{
			using (var stringWriter = new StringWriter())
			using (var jsonWriter = new JsonTextWriter(stringWriter))
			{
				var serializer = new JsonSerializer
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				};

				jsonWriter.QuoteName = false;
				serializer.Serialize(jsonWriter, value);

				return new HtmlString(stringWriter.ToString());

			}
		}
	}
}
