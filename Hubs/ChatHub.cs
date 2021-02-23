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
        public async Task ChatViewFromServer(string message)
        {
            Console.WriteLine("Receive posion from Server app :" + message + "/" );
            await Clients.Others.SendAsync("ReceiveNewPosition", message);
        }
        private IHubContext<ChatHub> HubContext{ get; set; }
    }
}
