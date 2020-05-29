using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class Car
    {
        public Car()
        {
            Job = new HashSet<Job>();
        }

        public int CarId { get; set; }
        public int UserId { get; set; }
        public string VehicleRegistration { get; set; }
        public int BrandId { get; set; }
        public int Model_Id { get; set; }
        public string Image { get; set; }

        public virtual CarBrand Brand { get; set; }
        public virtual ICollection<Job> Job { get; set; }
    }
}
