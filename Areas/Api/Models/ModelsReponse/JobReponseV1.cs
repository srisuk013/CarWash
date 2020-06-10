using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReponse
{
    public class JobReponseV1
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public JobRequset Job { get; set; }
    }
}
