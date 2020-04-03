using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class WalletLogs
    {
        public int WalletLogsId { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateTime { get; set; }
        public string Type { get; set; }
        public decimal? Amount { get; set; }
        public byte[] Image { get; set; }
        public string CConfirm { get; set; }
    }
}
