using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Hubs
{
    public class MoveViewHub: Hub
    {
        public  async Task MoveViewFromServer(float newX ,float newY)
        {
            Console.WriteLine("Receive posion from Server app :"+ newX+"/"+newY);
            await Clients.Others.SendAsync("ReceiveNewPosition", newX,newY);
        }
    }
}
