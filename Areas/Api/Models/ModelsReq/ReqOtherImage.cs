using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReq
{
    public class ReqOtherImage
    {
        public IFormFile file { get; set; }
      
    }
}
