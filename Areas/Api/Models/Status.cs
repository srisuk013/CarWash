using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class Status
    {
            public const int Active = 1;
            public const int InActive = 2;
            

            public class Desc
            {
                public const string Active = "Active";
                public const string InActive = "InActive";
                
            }
       
        
    }
}
