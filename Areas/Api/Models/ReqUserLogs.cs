using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class ReqUserLogs
    {
        public string LogsKeys { get; set; }

        public int LogsStatus { get; set; }
    }
}
