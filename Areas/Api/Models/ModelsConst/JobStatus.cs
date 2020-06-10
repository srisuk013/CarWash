using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsConst
{
    public class JobStatus
    {
        public const int BookingJob = 1;
        public const int RejectJob = 2;
        public const int ReceiveJob = 3;
        public const int Arrive = 4;
        public const int Report = 5;
        public const int Payment= 6;


        public class Desc
        {
            public const string RejectJob = "RejectJob";
            public const string BookingJob = "BookingJob";
            public const string ReceiveJob = "AcceptJob";
            public const string Arrive = "Arrive";
            public const string Report = "Report";
            public const string Payment = "Payment";

        }

    }
}
