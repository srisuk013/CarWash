using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ImageService
    {
        public string ImageFront;
        public string ImageBack;
        public string ImageLeft;
        public string ImageRight;
        public List<OtherImage> OtherImages;

        public ImageService()
        {
            this.OtherImages = new List<OtherImage>();
        }
    }
}
