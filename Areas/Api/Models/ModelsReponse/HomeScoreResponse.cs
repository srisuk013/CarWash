using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class HomeScoreResponse : BaseResponse
    {
        public HomeScoreModel HomeScore { get; set; }
    }
}
