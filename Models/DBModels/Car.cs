using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class Car
    {
        public int CarId { get; set; }
        public int UserId { get; set; }
        public string CarNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public byte[] Image { get; set; }
    }
}
