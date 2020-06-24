using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class HomeScore
    {
        public int HomeScoreId { get; set; }
        public double? Acceptance { get; set; }
        public double? Cancellation { get; set; }
        public double? Rating { get; set; }
        public int? MaxJob { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime CreatedTime { get; set; }
        public int? Score { get; set; }
        public int? Timeout { get; set; }

        public virtual User Employee { get; set; }
    }
}
