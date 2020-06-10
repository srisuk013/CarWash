using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class UserInfoResponse
    {

        public bool Success { get; set; }

        public string Message { get; set; }
        public UserInfo UserInfo { get; set; }

    }
}
