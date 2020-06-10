using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class OthrerImage
    {
        public int IdImage { get; set; }
        public string Image { get; set; }
        public int? JobId { get; set; }

        public virtual Job Job { get; set; }
    }
}
