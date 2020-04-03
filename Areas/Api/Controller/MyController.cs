using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using CarWash.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CarWash.Areas.Api.Models;

namespace CarWash.Areas.Api
{
    [Area("Api")]
    //[Authorize(Roles = "Employee, Customer")]
    public class MyController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityUser> _roleManager;
        private readonly CarWashContext _context;
        public MyController(CarWashContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityUser> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]

        public IActionResult UserList()

        {
            List<User> users = _context.User.ToList();
            List<CWUser> cwUsers = new List<CWUser>();
            foreach (User user in users)
            {
                CWUser cwUser = new CWUser(user);
                cwUsers.Add(cwUser);
            }
            Object result = new
            {
                Users = cwUsers
            };
            //IdentityUser newUser = new IdentityUser();
            //newUser.UserName = "aa";
            //newUser.Email = "ss";
            //newUser.PhoneNumber = "00";

            //IdentityResult createResult = await _userManager.CreateAsync(newUser, "xxx");
            //var addRoleResult = await _userManager.AddToRoleAsync(newUser, "Employee");
            //if(createResult == IdentityResult.Success)
            //{
            //    //return Ok();
            //}
            //else
            //{
            //    //return BadRequest();
            //}
            return Json(result);

        }
    }

}


                

        


 