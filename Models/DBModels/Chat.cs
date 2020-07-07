using System;
using System.Collections.Generic;

namespace CarWash.Models.DBModels
{
    public partial class Chat
    {
        public int ChatId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
