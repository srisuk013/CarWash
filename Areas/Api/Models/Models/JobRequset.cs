using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class JobRequset
    {
        public int JobId { get; set; }
        public string ImageProfile { get; set; }
        public string FullName { get; set; }
        public string PackageName { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public string VehicleRegistration { get; set; }
        public string Price { get; set; }
        public string DateTime { get; set; }

    }
}
