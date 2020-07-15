using CarWash.Areas.Api.Models.ModelsReponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Hubs
{
    public class JobMessage
    {
        public int JobId { get; set; }
        public string Phone { get; set; }
        public string ImageProfile { get; set; }
        public string FullName { get; set; }
        public string PackageName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string VehicleRegistration { get; set; }
        public string Price { get; set; }
        public string DateTime { get; set; }
        public string Distance { get; set; }
        public string Location { get; set; }
        public string Timeout { get; set; }

    }
}
