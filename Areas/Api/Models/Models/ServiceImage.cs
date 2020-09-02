using CarWash.Areas.Api.Models.ModelsReponse;
using CarWash.Models.DBModels;
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


        public List<OtherImage> OtherImagesService { get; set; }

        public ServiceImage()
        {

            OtherImagesService = new List<OtherImage>();
            FrontBefore = "";
            BackBefore = "";
            LeftBefore = "";
            RightBefore = "";
            FrontAfter = "";
            BackAfter = "";
            LeftAfter = "";
            RightAfter = "";
        }

        public ServiceImage(ImageService service)
        {
            FrontBefore = service.FrontBefore;
            BackBefore = service.BackBefore;
            LeftBefore = service.LeftBefore;
            RightBefore = service.RightBefore;
            FrontAfter = service.FrontAfter;
            BackAfter = service.BackAfter;
            LeftAfter = service.LeftAfter;
            RightAfter = service.RightAfter;
            OtherImagesService = new List<OtherImage>();
        }

    }
}
