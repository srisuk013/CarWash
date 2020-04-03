using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class Wallet
    {
        public int WalletId { get; set; }
        public int? UserId { get; set; }
        public string Balance { get; set; }
    }
}
