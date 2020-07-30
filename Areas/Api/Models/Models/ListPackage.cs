using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ListPackage
    {
        public int PackageId;
        public int SizeId;
        public string Packagename;
        public string Description;

        public ListPackage()
        {
        }

        public ListPackage(Package package)
        {
            this.PackageId = package.PackageId;
            this.Packagename = package.PackageName;
            this.Description = package.Description;
            this.SizeId = package.SizeId;
        }
    }
}
