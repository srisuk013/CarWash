using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class CarModels
    {
        public int Model_Id { get; set; }
        public int BrandId { get; set; }

        public string BrandName { get; set; }
        public int SizeId { get; set; }

        public string SizeName { get; set; }

        public string ModelName { get; set; }
    }
}
