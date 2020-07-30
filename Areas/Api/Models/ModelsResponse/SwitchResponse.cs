using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReponse
{
    public class SwitchResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public  int  SwitchFlag { get; set; }

    }
}
