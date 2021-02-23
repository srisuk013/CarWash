using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ListPackageAll
    {

       
       
        public string Price_s;

        public ListPackageAll()
        {
        }

        public ListPackageAll(Package package)
        {
            this.Price_s = package.Price.ToString();
        }
    }
}
