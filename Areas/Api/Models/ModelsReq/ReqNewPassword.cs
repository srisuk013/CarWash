using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class ReqNewPassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
