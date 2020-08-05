using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ListPackageAll
    {

        public string SizeName;
        public string Packagename;
        public string Price;

        public ListPackageAll()
        {
        }

        public ListPackageAll(Package package)
        {
            this.Packagename = package.ModelPackage.PackageName;
            this.SizeName = package.Size.SizeName;
            this.Price = package.Price.ToString();
        }
    }
}
