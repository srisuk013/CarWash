
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class ReqRegister
    {


        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
       // [RegularExpression(@"^([0-9])$",ErrorMessage = "ใส่เฉพาะตัวเลขPhone")]
        public string Phone { get; set; }

        //[RegularExpression(@"^([0-9])$", ErrorMessage = "ใส่เฉพาะตัวเลขIdCardNumber")]
        public string IdCardNumber { get; set; }

      //  [RegularExpression(@"^([1-3])$", ErrorMessage = "ใส่เฉพาะตัวเลขRole")]
        public int Role { get; set; }
        public IFormFile file  { get; set; }

    }

}




