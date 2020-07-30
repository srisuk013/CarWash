using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class UserInfoV1
    {
        public int UserId;
        public string FullName;
        public string IdCardNumber;
        public string Phone;
        public string Code;
        public string Image;

        public UserInfoV1()
        {
        }

        public  UserInfoV1(User user)
        {
            this.UserId = user.UserId;
            this.FullName = user.FullName;
            this.IdCardNumber = user.IdCardNumber;
            this.Phone = user.Phone;
            this.Code = user.Code;
            this.Image = user.Image;

        }

    }
}
