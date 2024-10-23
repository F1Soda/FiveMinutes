using FiveMinutes.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveMinutes.Models
{
	public class EducationTest
	{
		[Key]
		public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("FiveMinuteTemplate")]
        public int? AttachedFMTId {  get; set; }
		public FiveMinuteTemplate AttachedFMT { get; set; }
		public TestStatus Status { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; }

        [ForeignKey("AppUser")]
		public int? UserOrganizerId { get; set; }
	}
}
