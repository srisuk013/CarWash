using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public byte[] Image { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
