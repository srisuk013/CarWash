using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Hubs
{
    public class MoveViewHubV2: Hub
    {
        public  async Task MoveViewFromServer(string newN)
        {
            Console.WriteLine("Receive posion from Server app :" + newN);
            await Clients.Others.SendAsync("ReceiveNewPosition", newN);
        }
    }
}
