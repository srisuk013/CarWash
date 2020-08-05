using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsResponse
{
    public class ShowPackageAllResponse :BaseResponse
    {
        public List<ListPackageAll> PackageCarV1 { get; set; }

        public List<ListPackageAll> packageCar { get; set; }

    }
}
