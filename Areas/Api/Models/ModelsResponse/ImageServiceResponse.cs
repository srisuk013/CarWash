using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReponse
{
    public class ImageServiceResponse : BaseResponse
    {
        public ServiceImage ServiceImage { get; set; }
        public int ImageId { get; set; }
    }
}
