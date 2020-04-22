using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class State
    {
        public const int Offline = 0;
        public const int Online = 1;


        public class Desc
        {
            public const string offline = "Offline";
            public const string Online = "Online";

        }
    }
}
