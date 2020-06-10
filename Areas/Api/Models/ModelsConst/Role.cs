using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class Role
    {
        public const int Admin =1;
        public const int Customer =2;
        public const int Employee =3;
         
        public class Desc
        {
            public const string Admin = "Admin";
            public const string Customer = "Customer";
            public const string Employee = "Employee";
        }
    }
}
