using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
    }
}
