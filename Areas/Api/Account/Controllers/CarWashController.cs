
using CarWash.Areas.Api.Models;
using CarWash.Service;
using Microsoft.AspNetCore.Mvc;


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
    }
}
