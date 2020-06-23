using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class AfterImageSevice
    {
        public int ImageId { get; set; }
        public string Image { get; set; }
        public int? JobId { get; set; }
        public int? Type { get; set; }

        public virtual Job Job { get; set; }
    }
}
