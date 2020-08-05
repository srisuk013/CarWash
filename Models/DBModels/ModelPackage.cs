using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class ModelPackage
    {
        public ModelPackage()
        {
            Package = new HashSet<Package>();
        }

        public int ModelPackageId { get; set; }
        public string PackageName { get; set; }

        public virtual ICollection<Package> Package { get; set; }
    }
}
