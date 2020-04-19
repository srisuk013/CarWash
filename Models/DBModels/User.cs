using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string AspNetUserId { get; set; }
        public string AspNetRole { get; set; }
        public string Image { get; set; }
        public int Role { get; set; }
    }
}
