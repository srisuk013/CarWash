using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class Scores
    {
        public int ScoreId { get; set; }
        public int? ConfirmJob { get; set; }
        public int? CancleJob { get; set; }
        public int? EmployeeId { get; set; }

        public virtual User Employee { get; set; }
    }
}
