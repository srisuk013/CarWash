﻿using CarWash.Models.DBModels;
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
        public string Price;

        public ListPackage()
        {
        }

        public ListPackage(Package package)
        {
            this.PackageId = package.PackageId;
            this.Packagename = package.ModelPackage.PackageName;
            this.Description = package.Description;
            this.SizeId = package.SizeId;
            this.Price = package.Price.ToString();
        }
    }
}
