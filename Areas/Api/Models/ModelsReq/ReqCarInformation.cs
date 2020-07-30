using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ReqCarInformation
    {

        public int ImageId { get; set; } 
        public string  VehicleRegistration { get; set; } 

        public int province { get; set; }
         
        public int BaBrandId { get; set; }

        public int ModelId { get; set; }

        public string ImageCarUrl { get; set; }

    }
}
