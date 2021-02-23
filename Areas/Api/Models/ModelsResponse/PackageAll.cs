using CarWash.Areas.Api.Models.Models;
using CarWash.Models.DBModels;
using System.Collections.Generic;

namespace CarWash.Areas.Api.Models.ModelsResponse
{
  public class PackageAll
    {
        public List<ListPrice> price;
        public List<ListPackageName> packageNames;


        public PackageAll()
        {

        }
        public PackageAll(Package package)
        {
            this.packageNames = new List<ListPackageName>();
            this.price = new List<ListPrice>();

        }

    }
}