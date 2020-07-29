using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class Province
    {
        public Province()
        {
            Car = new HashSet<Car>();
        }

        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }

        public virtual ICollection<Car> Car { get; set; }
    }
}
