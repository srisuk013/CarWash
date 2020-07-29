using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class CarModel
    {
        public CarModel()
        {
            Car = new HashSet<Car>();
        }

        public int Model_Id { get; set; }
        public int? BrandId { get; set; }
        public string ModelName { get; set; }
        public int? SizeId { get; set; }

        public virtual CarBrand Brand { get; set; }
        public virtual CarSize Size { get; set; }
        public virtual ICollection<Car> Car { get; set; }
    }
}
