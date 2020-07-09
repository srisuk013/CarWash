using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class LocationReponse
    {
        public Meta meta { get; set; }
        public List<Datum> data { get; set; }

    }
}
