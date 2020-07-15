using System;

namespace CarWash.Areas.Api.Account.Controllers
{
    public class ReqBookingJob
    {
        public int PackageId { get; set; }
        public int CarId { get; set; }
        public double Latitude { get; set; }
       public double Longitude { get; set; }
    }
}