using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.Models.ModelsResponse;
using CarWash.Models.DBModels;
using CarWash.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Areas.Api.Account.Controllers
{
    [Area("Api")]
    [Route("Api/Chat/[Action]")]

    public class ChatController : Controller
    {

        private CarWashContext _context;
     

        public ChatController(CarWashContext context)
        {
            _context = context;
           
        }
        [HttpGet]

        public IActionResult fetchChat(string Name ,string Message)
        {
            ChatService service = new ChatService();
            ChatResponse response = service.fetchChat(_context, Name,Message);
            return Json(response);
        }

       

    }
}