using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsResponse
{
    public class UserResponse  :BaseResponse
    {
        public UserInfoV1 UserInfo { get; set; }
    }
}
