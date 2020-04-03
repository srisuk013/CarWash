using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class CWUser
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public CWUser(User user)
        {
            Name = user.Name;
            Username = user.Username;
            Password = user.Password;

        }
    }
}

