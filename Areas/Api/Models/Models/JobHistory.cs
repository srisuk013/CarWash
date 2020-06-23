using CarWash.Areas.Api.Models.Models;
using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public string JobDateTime;
        public string Comment;
        public List<OtherImage> OtherImageService;
        public List<ImageSevices> ImageBeforeService;
        public List<AfterImage> ImagesAfterService;







        public JobHistory()
        {
           
        }

        public JobHistory(Job job)
        {
            DateTime Date = job.JobDateTime;
            string dateStr = Date.ToString("dd/MM/yyyy HH:mm");
            this.JobId = job.JobId;
            this.ImageProfile = job.Customer.Image;
            this.FullName = job.Customer.FullName;
            this.PackageName = job.Package.PackageName;
            this.Latitude = job.Latitude;
            this.Longitude = job.Longitude;
            this.VehicleRegistration = job.Car.VehicleRegistration;
            this.Price = "฿ "+ job.Price+".00";
            this.JobDateTime = dateStr;
            this.ImageBeforeService = new List<ImageSevices>();
            this.Comment = job.Comment;
            this.ImagesAfterService = new List<AfterImage>();
            this.OtherImageService = new List<OtherImage>();

           
        }
   
    }
}
