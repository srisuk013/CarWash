using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ChooseMyCar
    {
        public int CarId;
        public string VehicleRegistration;
        public string Brand;

        public ChooseMyCar()
        {

        }

        public ChooseMyCar(Car car)
        {
            this.CarId = car.CarId;
            this.VehicleRegistration = car.VehicleRegistration;
            this.Brand = car.Brand.BrandName;

        }

    }
}
