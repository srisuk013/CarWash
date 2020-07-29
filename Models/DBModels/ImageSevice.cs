using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class ImageSevice
    {
        public int? ImageId { get; set; }
        public int? JobId { get; set; }
        public string FrontBefore { get; set; }
        public string BackBefore { get; set; }
        public string LaftBefore { get; set; }
        public string RightBefore { get; set; }
        public string FrontAfter { get; set; }
        public string BackAfter { get; set; }
        public string LaftAfter { get; set; }
        public string RightAfter { get; set; }
        public virtual Job Job { get; set; }
    }
}
