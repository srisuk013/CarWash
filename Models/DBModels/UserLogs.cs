﻿using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class UserLogs
    {
        public int UserLogsId { get; set; }
        public int UserId { get; set; }
        public DateTime DatetimeActiveIn { get; set; }
        public DateTime? DatetimeActiveOut { get; set; }

        public virtual User User { get; set; }
    }
}
