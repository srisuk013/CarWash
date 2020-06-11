using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReponse
{
    public class ImageServiceReponse  : BaseResponse
    {
         public ImageService ServiceImage { get; set; }
    }
}
