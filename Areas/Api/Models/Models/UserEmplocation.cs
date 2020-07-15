using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class UserEmplocation
    {
        public int State;
        public int UserId;
        public string Username;
        public int Role;
        public double? Latitude;
        public double? Longitude;
        public double Location;
        public double? Rating;




        public UserEmplocation(User user)
        {
            this.State = user.State;
            this.UserId = user.UserId;
            this.Username = user.Username;
            this.Role = user.Role;
            this.Latitude = user.Latitude;
            this.Longitude = user.Longitude;
            //this.Rating = homeScore.Rating;
           
           
        }                        

    }
}
