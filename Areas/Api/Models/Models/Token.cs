using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class Token
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
