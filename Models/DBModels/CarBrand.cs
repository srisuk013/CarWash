using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class CarBrand
    {
        public CarBrand()
        {
            Car = new HashSet<Car>();
            CarModel = new HashSet<CarModel>();
        }

        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public virtual ICollection<Car> Car { get; set; }
        public virtual ICollection<CarModel> CarModel { get; set; }
    }
}
