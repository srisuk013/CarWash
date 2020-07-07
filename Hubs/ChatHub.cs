using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Hubs
{
    public class ChatHub: Hub
    {
        public  async Task SendChat(string newN)
        {
            ChatMessage chat = new ChatMessage();
            Console.WriteLine("Receive posion from Server app :" + newN);
            await Clients.Others.SendAsync("ReceiveChat", newN);
        }
    }
}
