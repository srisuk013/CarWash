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

 /*       public  async  Task SendChat(string message)
        {
           
            ChatMessage json = new ChatMessage();

            json.ChatId = 1;
            json.Name = "not123456";
            json.Message = "ล้างรถ"+ message;
            string result = JsonConvert.SerializeObject(json);
            Console.WriteLine("Receive posion from Server app :" + result);
            await Clients.Others.SendAsync("ReceiveChat", result);
        }*/
    }
}
