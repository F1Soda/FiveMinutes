﻿using FiveMinutes.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveMinutes.Models
{
	public class Test
	{
		[Key]
		public int Id { get; set; }
        public string Name { get; set; }
		public int? AttachedFiveMinuteTemplateId {  get; set; }
		public FiveMinuteTemplate AttachedFiveMinuteTemplate { get; set; }
		public TestStatus Status { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; }

        [ForeignKey("AppUser")]
		public int? UserOrganizerId { get; set; }
	}
}
