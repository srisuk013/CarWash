using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class JobHistory

    {
        public string FullName;
        public string PackageName;
        public float Longitude;
        public float Latitude;
        public string VehieleRegistration;
        public DateTime JobDateTime;

        public JobHistory()
        {
        }

        public JobHistory(Job job)
        {
            this.FullName = job.Employee.FullName;
            this.PackageName = job.Package.PackageName;
            this.Latitude = job.Longitude.GetHashCode();
            this.Longitude = job.Longitude.GetHashCode();
            this.VehieleRegistration = job.Car.VehicleRegistration;
            this.JobDateTime = job.JobDateTime;
        }

    

    }
}
