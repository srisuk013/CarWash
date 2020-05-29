using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class Job
    {
        public int JobId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public int PackageId { get; set; }
        public int CarId { get; set; }
        public DateTime JobDateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string StatusName { get; set; }
        public string JobApprove { get; set; }
        public string CodeJob { get; set; }
        public string ImageRight { get; set; }
        public string ImageLeft { get; set; }
        public string ImageFront { get; set; }
        public string ImageBack { get; set; }
        public string Report { get; set; }
        public int Price { get; set; }

        public virtual Car Car { get; set; }
        public virtual User Employee { get; set; }
        public virtual Package Package { get; set; }
    }
}
