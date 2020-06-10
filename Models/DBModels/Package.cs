using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class Package
    {
        public Package()
        {
            Job = new HashSet<Job>();
        }

        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int SizeId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string PackageImage { get; set; }

        public virtual CarSize Size { get; set; }
        public virtual ICollection<Job> Job { get; set; }
    }
}
