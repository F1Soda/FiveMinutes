using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;

namespace FiveMinutes.Models
{
	public class User
	{
		[Key]
		public string Id { get; set; }
	}
}
