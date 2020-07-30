using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class HistoryResponse    :BaseResponse
    {

        public List<JobHistory> Histories { get; set; }
        


    }
}
