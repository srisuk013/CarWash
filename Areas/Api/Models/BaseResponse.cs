using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class BaseResponse
    {
        private bool success;
        private string message;

        public BaseResponse(Boolean success,string message)
        {
            this.success = success;
            this.message = message;

        }

    }
}
