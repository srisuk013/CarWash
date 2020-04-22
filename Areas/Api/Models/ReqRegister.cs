
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class ReqRegister
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public int Role { get; set; }
        public IFormFile Image { get; set; }
    }
}
       



