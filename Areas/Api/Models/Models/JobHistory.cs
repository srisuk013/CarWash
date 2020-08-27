using CarWash.Areas.Api.Models.Models;
using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;

namespace CarWash.Areas.Api.Models
{
    public class JobHistory

    {
        public int JobId;
        public string ImageProfile;
        public string FullName;
        public string PackageName;
        public string VehicleRegistration;
        public string Price;
        public string JobDateTime;
        public string Comment;
        public string Location;
        public List<OtherImage> OtherImagesService;
        public List<ImageSevices> ImagesBeforeService;
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
            this.PackageName = job.Package.ModelPackage.PackageName;
            this.VehicleRegistration = job.Car.VehicleRegistration;
            //this.Province = job.Car.Province.ProvinceName;
            this.Price = "฿ " + job.Package.Price + ".00";
            this.JobDateTime = dateStr;
            this.ImagesBeforeService = new List<ImageSevices>();
            this.Comment = job.Comment;
            this.Location = job.Location;
            this.ImagesAfterService = new List<AfterImage>();
            this.OtherImagesService = new List<OtherImage>();
        }

    }
}
