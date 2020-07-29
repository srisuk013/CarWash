using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ServiceImage
    {
        public string FrontBefore { get; set; }
        public string BackBefore { get; set; }
        public string LeftBefore { get; set; }
        public string RightBefore { get; set; }
        public string FrontAfter { get; set; }
        public string BackAfter { get; set; }
        public string LeftAfter { get; set; }
        public string RightAfter { get; set; }

        public int? ImageId { get; set; }
        public List<OtherImage> OtherImagesService { get; set; }

        public ServiceImage()
        {
            
            this.OtherImagesService = new List<OtherImage>();
           
        }
      


    }
}
