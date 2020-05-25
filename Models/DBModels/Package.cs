using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class Package
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int SizeId { get; set; }
        public byte[] PackageImage { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Description { get; set; }
    }
}
