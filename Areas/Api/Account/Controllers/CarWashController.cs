
using CarWash.Areas.Api.Models;
using CarWash.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarWash.Areas.Api.Account.Controllers
{
    public class CarWashController : Controller
    {
        public BaseResponse BaseResponse;
        public ServiceToken Service;
        public ConstUser constUser; 
        public SignInResponse SignInResponse;
        public ServiceCheck ServiceCheck;
/*        public int idName;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            idName = int.Parse(Id);

            base.OnActionExecuting(context);


        }*/

    }
}
