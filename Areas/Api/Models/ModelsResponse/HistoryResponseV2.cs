using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsResponse
{
    public class HistoryResponseV2   :BaseResponse
    {
        public List<HistoryCustomer> Histories { get; set; }
    }
}
