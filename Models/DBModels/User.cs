using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class User
    {
        public User()
        {
            JobCustomer = new HashSet<Job>();
            JobEmployee = new HashSet<Job>();
            UserLogs = new HashSet<UserLogs>();
            Wallet = new HashSet<Wallet>();
            WalletLogs = new HashSet<WalletLogs>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Code { get; set; }
        public int? Status { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string AspNetUserId { get; set; }
        public string AspNetRole { get; set; }
        public string Image { get; set; }
        public int Role { get; set; }
        public int State { get; set; }

        public virtual ICollection<Job> JobCustomer { get; set; }
        public virtual ICollection<Job> JobEmployee { get; set; }
        public virtual ICollection<UserLogs> UserLogs { get; set; }
        public virtual ICollection<Wallet> Wallet { get; set; }
        public virtual ICollection<WalletLogs> WalletLogs { get; set; }
    }
}
