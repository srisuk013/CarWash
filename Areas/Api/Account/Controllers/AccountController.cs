using CarWash.Areas.Api.Models;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using CarWash.Models;
using Microsoft.AspNetCore.Authorization;



namespace CarWash.Areas.Account
{
    [Area("Api")]
    //[Authorize(Roles = "Employee, Customer")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly CarWashContext _context;
        public AccountController(CarWashContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }
        [HttpPost]

        public async Task<IActionResult> Register([FromForm]ReqRegister req)

        {
            
            IdentityUser Users = new IdentityUser();
            Users.UserName = req.Username;
          

            IdentityResult result = await _userManager.CreateAsync(Users, req.Password);

            if (result == IdentityResult.Success)
            {
                User user = new User();
                user.FullName = req.FullName;
                user.Username = req.Username;
                user.Phone = req.IdCardNumber;
                user.IdCardNumber = req.IdCardNamber;
                _context.User.Add(user);
                _context.SaveChanges();
            }
            return Json(result);
                
           

        }

    }

}



                











          
          

                
                 










  



                

        


