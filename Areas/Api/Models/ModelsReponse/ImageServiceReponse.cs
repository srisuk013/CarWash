using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReponse
{
    public class ImageServiceReponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public ImageService Images { get; set; }
    }
}
