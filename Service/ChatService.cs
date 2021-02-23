using CarWash.Areas.Api.Models.ModelsResponse;
using CarWash.Models.DBModels;
using CarWash.Repositories;
using System;

namespace CarWash.Service
{
    public class ChatService
    {

        public ChatResponse fetchChat(CarWashContext context,string Name, string Message)
        {
            ChatResponse response = new ChatResponse();

            response.Success = false;

            if(String.IsNullOrEmpty(Name))
            {

                response.Message = "IsNullOrEmpty Name";
            }
            else if(String.IsNullOrEmpty(Message))
            {
                response.Message = "IsNullOrEmpty Message";
            }
            else
            {
                response.Success = true;
                response.Message = "Fetch chat success";

                DefaultRepositories repositories = new DefaultRepositories(context);
                response.Chat = repositories.fetchChat();
            }

            return response;

        }

    }
}
