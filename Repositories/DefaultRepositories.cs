using CarWash.Areas.Api.Models.Models;
using CarWash.Areas.Api.Models.ModelsResponse;
using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Repositories
{
    public class DefaultRepositories
    {

        private CarWashContext _context;

        public DefaultRepositories(CarWashContext context)
        {
            _context = context;
        }

        public List<ChatModel> fetchChat()
        {
            List<ChatModel> response = new List<ChatModel>();

            var list = _context.Chat.ToList();
            foreach(Chat chat in list)
            {
                ChatModel model = new ChatModel();
                model.Name = chat.Name;
                model.Message = chat.Message;
                response.Add(model);
            }

            return response;

        }


    }
}
