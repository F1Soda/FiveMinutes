﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FiveMinute.Data;
using FiveMinute.Models;

namespace FiveMinute.Models
{
	public class FiveMinuteTest
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		[ForeignKey("FiveMinuteTemplate")]
		public int? AttachedFMTId { get; set; }
		public FiveMinuteTemplate AttachedFMT { get; set; }
		public TestStatus Status { get; set; }

		public bool StartPlanned = false;
		public DateTime StartTime { get; set; }
		public bool EndPlanned = false;
		public DateTime EndTime { get; set; }

		public List<int> PositionsToInclude { get; set; }

		public IEnumerable<FiveMinuteTestResult> Results { get; set; }

		[ForeignKey("AppUser")]
		public string? UserOrganizerId { get; set; }
		public AppUser? UserOrganizer { get; set; }
	}
}