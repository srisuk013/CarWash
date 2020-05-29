using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class CarSize
    {
        public CarSize()
        {
            CarModel = new HashSet<CarModel>();
            Package = new HashSet<Package>();
        }

        public int SizeId { get; set; }
        public string SizeName { get; set; }

        public virtual ICollection<CarModel> CarModel { get; set; }
        public virtual ICollection<Package> Package { get; set; }
    }
}
