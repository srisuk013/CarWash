using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class State
    {
        public const int Off = 0;
        public const int On = 1;


        public class Desc
        {
            public const string off = "Offline";
            public const string On = "Online";

        }
    }
}
