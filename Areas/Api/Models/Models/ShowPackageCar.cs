﻿using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ShowPackageCar
    {

        public List<ListPackage> ListPackages { get; set; }

        public ShowPackageCar()
        {
            this.ListPackages = new List<ListPackage>();
        }

    }
}
