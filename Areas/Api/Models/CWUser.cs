using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class CWUser
    {
        private User user;

        public CWUser(User user)
        {
            this.user = user;
        }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


    }
}






