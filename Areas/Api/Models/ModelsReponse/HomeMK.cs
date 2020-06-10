using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReponse
{
    public class HomeMK
    {
        public Boolean Success { get; set; }
        public string Message { get; set; }

        public HomeMok HomeScore { get; set; }
    }
}
