using CarWash.Areas.Api.Models.Models;
using CarWash.Models.DBModels;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class JobHistory

    {
        public int JobId;
        public string ImageProfile;
        public string FullName;
        public string PackageName;
        public Double Latitude;
        public Double Longitude;
        public string VehicleRegistration;
        public string Price;
        public DateTime JobDateTime;
        public string ImageRight;
        public string ImageLeft;
        public string ImageFront;
        public string ImageBack;
        public string Comment;
        public List<OtherImage> OtherImages;




        public JobHistory()
        {
        }

        public JobHistory(Job job)
        {
            this.JobId = job.JobId;
            this.ImageProfile = job.Customer.Image;
            this.FullName = job.Customer.FullName;
            this.PackageName = job.Package.PackageName;
            this.Latitude = job.Latitude;
            this.Longitude = job.Longitude;
            this.VehicleRegistration = job.Car.VehicleRegistration;
            this.Price = "฿ " + job.Price +".00";
            this.JobDateTime = job.JobDateTime.Date;
            this.ImageFront = job.ImageFront;
            this.ImageBack = job.ImageBack;
            this.ImageLeft = job.ImageLeft;
            this.ImageRight = job.ImageRight;
            this.Comment = job.Comment;
            this.OtherImages = new List<OtherImage>();
        }
   
    }
}
