using CarWash.Models.DBModels;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Hubs
{
    public class ChatHub : Hub
    {
        private IHubContext<ChatHub> HubContext{ get; set; }
    }
}
