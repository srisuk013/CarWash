using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class AverageScore
    {
        public AverageScore()
        {
            Job = new HashSet<Job>();
        }

        public int AverageId { get; set; }
        public int EmployeeId { get; set; }
        public int Acceptance { get; set; }
        public int Cancellation { get; set; }
        public int score { get; set; }

        public virtual ICollection<Job> Job { get; set; }
    }
}
