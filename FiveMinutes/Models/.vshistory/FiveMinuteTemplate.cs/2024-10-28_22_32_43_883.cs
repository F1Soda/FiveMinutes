﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FiveMinutes.Models
{
	public class FiveMinuteTemplate
	{
		[Key]
		public int Id { get; set; }
		public string? Name { get; set; }

		public DateTime? CreationTime { get; set; }
		public DateTime? LastModificationTime { get; set; }
		public IEnumerable<Question> Questions { get; set; }
        public bool? ShowInProfile { get; set; }

        [ForeignKey("AppUser")]
        public string? UserOwnerId { get; set; }
        public AppUser? UserOwner { get; set; }
	}

}
