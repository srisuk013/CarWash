using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class SignInResponse
    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public Token Token { get; set; }

        public UserInfoV1 UserInfo { get; set; }


    }
}
