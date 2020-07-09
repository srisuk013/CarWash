using System.Collections.Generic;

namespace CarWash.Areas.Api.Models.Models
{
    public class Datum
    {
        public double fdistance { get; set; }
        public double tdistance { get; set; }
        public int id { get; set; }
        public List<Guide> guide { get; set; }
        public int distance { get; set; }
        public int interval { get; set; }
        public int penalty { get; set; }

    }
}