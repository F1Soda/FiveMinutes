﻿using System.ComponentModel.DataAnnotations;

namespace FiveMinutes.Models
{
    public class Folder
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder ParentFolder { get; set; }
        public ICollection<Folder> SubFolders { get; set; }
        public ICollection<EducationTest> Tests { get; set; }
    }
}
